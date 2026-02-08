using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class DictService : IDictService
    {
        private readonly PmcContext _context;
        private readonly IConfiguration _configuration;

        public DictService(PmcContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region 1. 查询 (GetTableData)

        public async Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null)
        {
            // 1. 获取配置
            var config = GetConfig(type);

            if (string.IsNullOrEmpty(config.ViewName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 ViewName，无法查询数据。");

            string viewName = config.ViewName;
            var result = new DictTableDto();

            // ✅ 核心修复：必须填充 Columns，否则前端不知道怎么渲染表头
            result.Columns = config.Columns.Select(c => new DictColumnDto
            {
                Prop = c.DbField,
                Label = c.Title,
                Show = !c.IsHidden,
                UiType = c.UiType ?? "Input",
                Required = c.IsRequired,
                IsPrimaryKey = c.IsPrimaryKey,
                IsReadOnly = c.IsReadOnly,
                // 映射下拉框配置对象
                DataSource = c.DataSource == null ? null : new DictDataSourceDto
                {
                    Url = c.DataSource.Url,
                    LabelField = c.DataSource.LabelField,
                    ValueField = c.DataSource.ValueField,
                    ValueMapping = c.DataSource.ValueMapping
                }
            }).ToList();

            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            // 2. 构建基础 SQL
            string sql = $"SELECT * FROM [{viewName}] WHERE 1=1";
            var parameters = new DynamicParameters();

            // 3. 处理关键字搜索
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var searchConditions = new List<string>();

                // 只对 Input 类型且未隐藏的列进行模糊搜索，防止对非文本列进行 LIKE 操作报错
                foreach (var col in config.Columns.Where(c => c.UiType == "Input" && !c.IsHidden))
                {
                    // 建议：如果全是文本列可以直接用；如果有数字列，SQL Server 需要 CAST([...T] as NVARCHAR)
                    searchConditions.Add($"[{col.DbField}] LIKE @Kw");
                }

                if (searchConditions.Any())
                {
                    sql += " AND ( " + string.Join(" OR ", searchConditions) + " )";
                    parameters.Add("Kw", $"%{keyword}%");
                }
            }

            // 4. 排序 (默认按 ID 倒序，防止无序跳动)
            // 确保视图里有 ID 列，如果没有 ID 列，这里需要根据 IsPrimaryKey 配置动态找
            if (config.Columns.Any(c => c.DbField == "ID"))
            {
                sql += " ORDER BY ID DESC";
            }

            // 5. 执行查询
            var rows = await conn.QueryAsync(sql, parameters);

            // 6. 转换结果
            result.Rows = rows
                .Select(row => (IDictionary<string, object>)row)
                .Select(d => new Dictionary<string, object>(d))
                .ToList();

            // 7. 处理 JsonData (扩展列展平)
            foreach (var row in result.Rows)
            {
                if (row.ContainsKey("JsonData") && row["JsonData"] is string jsonStr && !string.IsNullOrEmpty(jsonStr))
                {
                    try
                    {
                        var jsonObj = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
                        if (jsonObj != null)
                        {
                            foreach (var kvp in jsonObj)
                            {
                                if (!row.ContainsKey(kvp.Key))
                                {
                                    row[kvp.Key] = kvp.Value;
                                }
                            }
                        }
                    }
                    catch { /* 忽略 Json 解析错误 */ }
                }
            }

            return result;
        }

        #endregion

        #region 2. 新增 (Add)

        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            var config = GetConfig(type);
            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();

            // 1. 获取物理表的列 (S3D_Dict_...)
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            // 2. 获取配置文件中定义的所有列 (包含 Short, Long 等视图列)
            // 💡 关键点：建立一个“已知列”的清单
            var definedColSet = config.Columns
                .Select(c => c.DbField)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var insertDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            foreach (var kvp in data)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;
                if (kvp.Key.Equals("ID", StringComparison.OrdinalIgnoreCase)) continue;

                // A. 如果是物理列 -> 存入 SQL
                if (dbColSet.Contains(kvp.Key))
                {
                    insertDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                // B. 💡 关键修正：如果不是物理列，但在 dicts.json 里定义过 (如 Short, Long)
                // -> 说明它是视图展示列，直接忽略 (Do Nothing)，千万别进 JSON！
                else if (definedColSet.Contains(kvp.Key))
                {
                    continue;
                }
                // C. 既不是物理列，也没在配置里定义 -> 才是真正的前端新增列 (Extension)
                // -> 存入 JsonData
                else
                {
                    jsonDict[kvp.Key] = kvp.Value;
                }
            }

            // 3. 处理 JsonData
            if (jsonDict.Any() && dbColSet.Contains("JsonData"))
            {
                insertDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
            }

            // 4. 自动填充系统字段
            if (dbColSet.Contains("CreatedTime")) insertDict["CreatedTime"] = DateTime.Now;
            if (dbColSet.Contains("CreatedBy")) insertDict["CreatedBy"] = "System"; // 后面可以改成当前用户
            if (dbColSet.Contains("Status") && !insertDict.ContainsKey("Status")) insertDict["Status"] = 1;

            // 5. 生成 SQL 并执行
            var colNames = insertDict.Keys.Select(c => $"[{c}]");
            var paramNames = insertDict.Keys.Select(c => $"@{c}");

            string sql = $"INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)}); SELECT CAST(SCOPE_IDENTITY() as int)";

            return await conn.QuerySingleAsync<int>(sql, new DynamicParameters(insertDict));
        }

        #endregion

        #region 3. 修改 (Update)

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            var config = GetConfig(type);
            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName");

            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();

            // 1. 获取物理表的列 (S3D_Dict_...)
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            // 2. 获取配置文件中定义的“已知列” (包含 Short, Long 等视图列)
            // 用于识别哪些是“展示字段”，需要被忽略
            var definedColSet = config.Columns
                .Select(c => c.DbField)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var updateDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            foreach (var kvp in data)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;
                // 排除主键，防止用户尝试修改 ID
                if (kvp.Key.Equals("ID", StringComparison.OrdinalIgnoreCase)) continue;

                // A. 如果是物理列 -> 更新它
                if (dbColSet.Contains(kvp.Key))
                {
                    updateDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                // B. 如果不是物理列，但在 dicts.json 里定义过 -> 说明是视图展示列，忽略！
                else if (definedColSet.Contains(kvp.Key))
                {
                    continue;
                }
                // C. 既不是物理列，也没配置过 -> 认为是扩展数据，进 JsonData
                else
                {
                    jsonDict[kvp.Key] = kvp.Value;
                }
            }

            // 3. 处理 JsonData (Update 时通常是覆盖整个 JSON)
            // 如果你想做“局部更新” (Merge)，逻辑会复杂很多，这里先按“前端传什么就存什么”处理
            if (jsonDict.Any() && dbColSet.Contains("JsonData"))
            {
                updateDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
            }
            else if (dbColSet.Contains("JsonData") && !jsonDict.Any())
            {
                // 如果前端传了数据但把扩展字段都清空了，也可以考虑把数据库的 JsonData 置空
                // updateDict["JsonData"] = DBNull.Value; // 视业务需求而定，暂时不强制清空
            }

            // 4. 自动更新时间
            if (dbColSet.Contains("UpdatedTime")) updateDict["UpdatedTime"] = DateTime.Now;
            // 记录修改人 (如果实现了用户系统，这里填当前用户)
            if (dbColSet.Contains("UpdatedBy")) updateDict["UpdatedBy"] = "System";

            if (!updateDict.Any()) return 0; // 没有有效字段需要更新

            // 5. 构造 SQL
            var setClauses = updateDict.Keys.Select(k => $"[{k}] = @{k}");
            string sql = $"UPDATE [{tableName}] SET {string.Join(", ", setClauses)} WHERE ID = @Id";

            var paramsDict = new DynamicParameters(updateDict);
            paramsDict.Add("Id", id);

            return await conn.ExecuteAsync(sql, paramsDict);
        }

        #endregion



        #region 4. 删除 (Delete)

        public async Task<int> DeleteAsync(string type, int id)
        {
            var config = GetConfig(type);
            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName");

            string tableName = config.PhysicalTableName;
            using var conn = _context.Database.GetDbConnection();

            string sql = $"DELETE FROM [{tableName}] WHERE ID = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        #endregion

        #region 辅助方法

        private DictItemConfig GetConfig(string type)
        {
            var config = _configuration.GetSection($"DictConfiguration:{type}").Get<DictItemConfig>();
            if (config == null)
            {
                throw new Exception($"未找到类型 '{type}' 的配置信息，请检查 dicts.json");
            }
            return config;
        }

        private async Task<List<string>> GetTableSchemaAsync(IDbConnection conn, string tableName)
        {
            if (conn.State != ConnectionState.Open)
            {
                if (conn is System.Data.Common.DbConnection dbConn) await dbConn.OpenAsync();
                else conn.Open();
            }
            var result = await conn.QueryAsync<string>(
                @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName",
                new { TableName = tableName });
            return result.ToList();
        }

        private object DataToSqlValue(object val)
        {
            if (val is JsonElement je)
            {
                return je.ValueKind switch
                {
                    JsonValueKind.String => je.GetString(),
                    JsonValueKind.Number => je.GetDecimal(),
                    JsonValueKind.True => 1,
                    JsonValueKind.False => 0,
                    JsonValueKind.Null => null,
                    _ => je.ToString()
                };
            }
            if (val is bool b) return b ? 1 : 0;
            return val;
        }

        #endregion
    }
}