using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.CodelistTable;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class CodelistService : ICodelistService
    {
        private readonly ILogger<CodelistService> _logger;
        private readonly AppDbContext _pmcContext;

        private static readonly Dictionary<string, int> _codelistTableCache = new();
        private static readonly Dictionary<string, Dictionary<int, string>> _codelistValueCache = new();
        private static DateTime _lastCacheRefresh = DateTime.MinValue;
        private static readonly TimeSpan _cacheRefreshInterval = TimeSpan.FromMinutes(30);

        public CodelistService(ILogger<CodelistService> logger, AppDbContext pmcContext)
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

        /// <summary>
        /// 根据列名和代码值列表，批量获取描述
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="codelistNumbers"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 根据父级Codelist值，获取所有子Codelist值
        /// </summary>
        /// <param name="parentTableName"></param>
        /// <param name="parentCodeNumber"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<CodeListItem>>> GetChildCodeListsByParentValueAsync(string parentTableName, int parentCodeNumber)
        {
            var result = new Dictionary<string, List<CodeListItem>>();

            try
            {
                //1. 获取父表ID
                var parentTableId = await GetCodelistTableIdAsync(parentTableName);

                if (parentTableId == 0)
                {
                    _logger.LogWarning($"未找到父表:{parentTableName}");
                    return result;
                }

                //2. 验证父值存在
                var parentCodeExists = await _pmcContext.S3dCommonCodeListValues
                    .AnyAsync(v => v.CodeListTableId == parentTableId && v.CodeListNumber == parentCodeNumber);

                if ( !parentCodeExists)
                {
                    _logger.LogWarning($"父表{parentTableName}中未找到代码值:{parentCodeNumber}");
                    return result;
                }

                //3. 查找所有直接子表
                var childTables = await _pmcContext.S3dCommonCodeListHierarchies
                    .Where(h => h.ParentCodeListTableId == parentTableId)
                    .Join(
                        _pmcContext.S3dCommonCodeListTables,
                        hierachy => hierachy.CodeListTableId,
                        table => table.Id,
                        (hierachy, table) => new { table.CodeListTableName, table.Id }).ToListAsync();

                if (!childTables.Any())
                {
                    _logger.LogInformation($"父表{parentTableName}没有直接子表");
                    return result;
                }

                // 4. 查询所有子表中ParentCodelistNumber等于父代码值的记录
                var childTableIds = childTables.Select(t => t.Id).ToList();

                var childValues = await _pmcContext.S3dCommonCodeListValues
                    .Where(v => childTableIds.Contains(v.CodeListTableId)
                        && v.ParentCodeListNumber == parentCodeNumber)
                    .OrderBy(v => v.CodeListTableId)
                    .ThenBy(v => v.CodeListNumber)
                    .ToListAsync();

                // 5. 按子表分组
                foreach (var childTable in childTables)
                {
                    var tableItems = childValues
                        .Where(v => v.CodeListTableId == childTable.Id)
                        .Select(v => new CodeListItem
                        {
                            Id = v.Id,
                            CodeListNumber = v.CodeListNumber,
                            ShortStringValue = v.ShortStringValue,
                            LongStringValue = v.LongStringValue,
                            ParentCodeListNumber = v.ParentCodeListNumber,
                            IsUserDefine = v.IsUserDefine,
                            Status = v.Status
                        })
                        .ToList();

                    if (tableItems.Any())
                    {
                        result[childTable.CodeListTableName] = tableItems;
                    }
                }

                _logger.LogInformation($"找到父表{parentTableName}({parentCodeNumber})的 {result.Count} 个子表数据");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取子Codelist失败，父表：{ParentTableName}，父代码值：{ParentCodeNumber}", parentTableName, parentCodeNumber);
                return result;
            }
        }

        /// <summary>
        /// 获取直接子表
        /// </summary>
        /// <param name="parentTableName"></param>
        /// <returns></returns>
        public async Task<List<CodeListTableInfo>> GetDirectChildTablesAsync(string parentTableName)
        {
            var result = new List<CodeListTableInfo>();

            try
            {
                // 1. 获取父表ID
                var parentTableId = await GetCodelistTableIdAsync(parentTableName);

                if (parentTableId == 0)
                {
                    _logger.LogWarning($"未找到父表:{parentTableName}");
                    return result;
                }

                // 2. 查询直接子表
                var childTables = await _pmcContext.S3dCommonCodeListHierarchies
                    .Where(h => h.ParentCodeListTableId == parentTableId)
                    .Join(
                        _pmcContext.S3dCommonCodeListTables,
                        hierachy => hierachy.CodeListTableId,
                        table => table.Id,
                        (hierachy, table) => new CodeListTableInfo
                        {
                            Id = table.Id,
                            TableName = table.CodeListTableName,
                            IsUserDefined = table.IsUserDefined,
                            Major = table.Major
                        }).ToListAsync();
                return childTables;
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"获取直接子表失败：{parentTableName}");
                return result;
            }
        }

        /// <summary>
        /// 获取直接父表
        /// </summary>
        /// <param name="childTableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<CodeListTableInfo>> GetParentTablesAsync(string childTableName)
        {
            var result = new List<CodeListTableInfo>();

            try
            {
                // 1. 获取子表ID
                var childTableId = await GetCodelistTableIdAsync(childTableName);

                if (childTableId == 0)
                {
                    _logger.LogWarning($"未找到子表:{childTableName}");
                    return result;
                }

                // 2. 查询父表
                var parentTables = await _pmcContext.S3dCommonCodeListHierarchies
                    .Where(h => h.CodeListTableId == childTableId)
                    .Join(
                        _pmcContext.S3dCommonCodeListTables,
                        hierachy => hierachy.ParentCodeListTableId,
                        table => table.Id,
                        (hierachy, table) => new CodeListTableInfo
                        {
                            Id = table.Id,
                            TableName = table.CodeListTableName,
                            IsUserDefined = table.IsUserDefined,
                            Major = table.Major
                        }).ToListAsync();
                return parentTables;
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"获取父表失败：{childTableName}");
                return result;
            }
            throw new NotImplementedException();
        }
    }
}
