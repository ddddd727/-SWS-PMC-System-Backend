using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class CodelistService : ICodelistService
    {
        private readonly ILogger<CodelistService> _logger;
        private readonly PmcContext _pmcContext;

        private static readonly Dictionary<string, int> _codelistTableCache = new();
        private static readonly Dictionary<string, Dictionary<int, string>> _codelistValueCache = new();
        private static DateTime _lastCacheRefresh = DateTime.MinValue;
        private static readonly TimeSpan _cacheRefreshInterval = TimeSpan.FromMinutes(30);

        public CodelistService(ILogger<CodelistService> logger, PmcContext pmcContext)
        {
            _logger = logger;
            _pmcContext = pmcContext;
        }

        /// <summary>
        /// 获取Codelist描述
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="codelistValue"></param>
        /// <returns></returns>
        public async Task<string> GetCodelistDescriptionAsync(string columnName, int codelistValue)
        {
            try
            {
                // 1. 解析表名
                var codelistTableName = ParseCodelistTableName(columnName);

                // 2. 获取或者缓存CodelistTableId
                var codelistTableId = await GetCodelistTableIdAsync(codelistTableName);

                if (codelistTableId == 0)
                {
                    _logger.LogWarning("未找到Codelist表：{CodelistTableName}", codelistTableName);
                    return null;
                }

                // 3. 获取描述
                var description = await GetDescriptionFronCacheOrDbAsync(codelistTableName, codelistTableId, codelistValue);

                return description ?? string.Empty;
            }catch( Exception ex)
            {
                _logger.LogError(ex, "获取Codelist描述失败，列名：{ColumnName}，值：{CodelistValue}", columnName, codelistValue);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取Codelist描述 （重载，字符串值）
        /// </summary>
        /// <param name="codelistTableName"></param>
        /// <param name="codelistValue"></param>
        /// <returns></returns>
        public Task<string?> GetShortDesciptionByCodelistValue(string codelistTableName, string codelistValue)
        {
            if(int.TryParse(codelistValue, out int codelistNumber))
            {
                return GetCodelistDescriptionAsync(codelistTableName, codelistNumber);
            }

            _logger.LogWarning("Codelist值无法转换为整数：{CodelistValue}", codelistValue);
            return null;
        }


        /// <summary>
        /// 解析列名，获取对应的Codelist表名
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string ParseCodelistTableName(string columnName)
        {
            // 移除“_cl”后缀
            if (columnName.EndsWith("_cl", StringComparison.OrdinalIgnoreCase))
            {
                return columnName.Substring(0, columnName.Length - 3);
            }

            // 如果列名不包含“_cl”后缀，则直接返回原列名
            return columnName;
        }


        /// <summary>
        /// 获取Codelist表的ID（带缓存）
        /// </summary>
        /// <param name="codelistTableName"></param>
        /// <returns></returns>
        private async Task<int> GetCodelistTableIdAsync(string codelistTableName)
        {
            // 检查缓存
            if (_codelistTableCache.TryGetValue(codelistTableName, out var cachedId))
            {
                return cachedId;
            }

            // 刷新缓存 （如果超过间隔时间）
            if (DateTime.UtcNow - _lastCacheRefresh > _cacheRefreshInterval)
            {
                await RefreshCodelistTableCacheAsync();
            }

            // 再次尝试从缓存获取
            if (_codelistTableCache.TryGetValue(codelistTableName, out cachedId))
            {
                return cachedId;
            }

            // 查询数据库
            var codelistTable = await _pmcContext.S3dCommonCodeListTables
                 .FirstOrDefaultAsync(clt => clt.CodeListTableName.Equals(codelistTableName, StringComparison.OrdinalIgnoreCase));

            if (codelistTable != null)
            {
                _codelistTableCache[codelistTableName] = codelistTable.Id;
                return codelistTable.Id;
            }
            return 0;
        }


        /// <summary>
        /// 刷新Codelist表缓存
        /// </summary>
        /// <returns></returns>
        private async Task RefreshCodelistTableCacheAsync()
        {
            try
            {
                var tables = await _pmcContext.S3dCommonCodeListTables
                    .Select(t => new { t.CodeListTableName, t.Id })
                    .ToListAsync();

                lock (_codelistTableCache)
                {
                    _codelistTableCache.Clear();
                    foreach (var table in tables)
                    {
                        _codelistTableCache[table.CodeListTableName] = table.Id;
                    }
                    _lastCacheRefresh = DateTime.UtcNow;
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex, "刷新Codelist表缓存失败");
            }
        }

        private async Task<string> GetDescriptionFronCacheOrDbAsync(string codelistTableName, int codelistTableId, int codelistNumber)
        {
            // 检查值缓存
            if (_codelistValueCache.TryGetValue(codelistTableName, out var codeValueDict)
                && codeValueDict.TryGetValue(codelistNumber, out var cachedDescription))
            {
                return cachedDescription;
            }

            // 从数据库查询
            var codelistValue = await _pmcContext.S3dCommonCodeListValues
                .FirstOrDefaultAsync(v => v.CodeListTableId == codelistTableId && v.CodeListNumber == codelistNumber);

            if (codelistValue != null)
            {
                // 更新缓存
                UpdateCache(codelistTableName, new Dictionary<int, string>
                {
                    {codelistNumber, codelistValue.ShortStringValue }
                });

                return codelistValue.ShortStringValue;
            }
            return null;
        }


        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="codelistTableName"></param>
        /// <param name="newValues"></param>
        private void UpdateCache(string codelistTableName, Dictionary<int, string> newValues)
        {
            lock (_codelistValueCache)
            {
                if (!_codelistValueCache.TryGetValue(codelistTableName, out var existingValues))
                {
                    existingValues = new Dictionary<int, string>();
                    _codelistValueCache[codelistTableName] = existingValues;
                }
                foreach (var kvp in newValues)
                {
                    existingValues[kvp.Key] = kvp.Value;
                }
            }

        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_codelistTableCache)
            {
                _codelistTableCache.Clear();
                _codelistValueCache.Clear();
                _lastCacheRefresh = DateTime.MinValue;
            }
        }

        public async Task<Dictionary<int, string>> GetCodelistDescriptionsAsync(string columnName, IEnumerable<int> codelistNumbers)
        {
            var result = new Dictionary<int, string>();

            try
            {
                // 1. 解析表名
                var codelistTableName = ParseCodelistTableName(columnName);

                // 2. 获取或者缓存CodelistTableId
                var codelistTableId = await GetCodelistTableIdAsync(codelistTableName);

                if (codelistTableId == 0)
                {
                    return result;
                }

                // 3. 批量查询
                var codes = codelistNumbers.Distinct().ToList();
                var descriptions = await _pmcContext.S3dCommonCodeListValues
                    .Where(v => v.CodeListTableId == codelistTableId && codes.Contains(v.CodeListNumber))
                    .Select(v => new
                    {
                        v.CodeListNumber,
                        v.ShortStringValue
                    })
                    .ToDictionaryAsync(v => v.CodeListNumber, v => v.ShortStringValue);

                // 更新缓存
                UpdateCache(codelistTableName, descriptions);
                return descriptions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量获取Codelist描述失败，列名：{ColumnName}", columnName);
                return result;
            }
        }
    }
}
