using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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

        #region 1. 查询 (GetTableData) - 使用 ViewName

        public async Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null)
        {
            // 1. 获取配置
            var config = GetConfig(type);

            // ✅ 使用 ViewName 查询 (已经 JOIN 好了 Short/Long)
            if (string.IsNullOrEmpty(config.ViewName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 ViewName，无法查询数据。");

            string viewName = config.ViewName;
            var result = new DictTableDto();

            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            // 2. 构建基础 SQL
            string sql = $"SELECT * FROM [{viewName}] WHERE 1=1";
            var parameters = new DynamicParameters();

            // 3. 处理关键字搜索 (简单模糊匹配)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                // 注意：这里假设您的视图里有 Short/Long 列，或者您可以根据配置里的 Columns 动态生成搜索条件
                // 这里做一个简单的通用搜索示例，搜索所有文本列
                // 实际生产建议根据 config.Columns 里的 IsSearchable 属性来拼
                sql += " AND ( ";
                var searchConditions = new List<string>();

                // 简单起见，假设我们搜索 Short 和 Long 列 (您可以根据实际需求调整)
                // 更好的做法是遍历 config.Columns 找到 UiType="Input" 的列
                foreach (var col in config.Columns.Where(c => c.UiType == "Input" && !c.IsHidden))
                {
                    searchConditions.Add($"[{col.DbField}] LIKE @Kw");
                }

                if (searchConditions.Any())
                {
                    sql += string.Join(" OR ", searchConditions);
                    sql += " )";
                    parameters.Add("Kw", $"%{keyword}%");
                }
                else
                {
                    // 如果没找到可搜索列，去掉 AND
                    sql = sql.Substring(0, sql.Length - 7);
                }
            }

            // 4. 排序 (默认按 ID 倒序)
            sql += " ORDER BY ID DESC";

            // 5. 执行查询
            var rows = await conn.QueryAsync(sql, parameters);

            // 6. 转换结果为字典列表
            result.Rows = rows
             .Select(row => (IDictionary<string, object>)row) // 先转成接口
             .Select(d => new Dictionary<string, object>(d))  // ✅ 关键：根据接口创建具体的 Dictionary 对象
               .ToList();

            // 7. 处理 JsonData (如果在 View 里是以字符串形式出来的，前端可能需要对象)
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
                                // 把 JsonData 里的 key 展平到 row 里，方便前端 grid 显示
                                // 前端列配置里配了 "JsonData" 对应的 Title，或者单独配了 Key
                                if (!row.ContainsKey(kvp.Key))
                                {
                                    row[kvp.Key] = kvp.Value;
                                }
                            }
                        }
                    }
                    catch { /* 忽略解析错误 */ }
                }
            }

            return result;
        }

        #endregion

        #region 2. 新增 (Add) - 使用 PhysicalTableName

        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            var config = GetConfig(type);

            // ✅ 使用 PhysicalTableName 写入 (只存 ID)
            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName，无法新增数据。");

            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();

            // 获取表结构，用于区分物理列和扩展列
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            // 分拣数据
            var insertDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            foreach (var kvp in data)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;
                if (kvp.Key == "ID") continue; // 自增ID不插

                if (dbColSet.Contains(kvp.Key))
                {
                    // 物理列直接存
                    insertDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                else if (dbColSet.Contains("JsonData"))
                {
                    // 非物理列放入 JsonData
                    jsonDict[kvp.Key] = kvp.Value;
                }
            }

            // 打包 JsonData
            if (jsonDict.Any() && dbColSet.Contains("JsonData"))
            {
                insertDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
            }

            // 自动填充系统字段
            if (dbColSet.Contains("CreatedTime")) insertDict["CreatedTime"] = DateTime.Now;
            if (dbColSet.Contains("CreatedBy")) insertDict["CreatedBy"] = "System"; // TODO: 换成真实用户
            if (dbColSet.Contains("Status") && !insertDict.ContainsKey("Status")) insertDict["Status"] = 1;

            if (!insertDict.Any()) throw new Exception("没有有效的数据可插入");

            // 构建 SQL
            var colNames = insertDict.Keys.Select(c => $"[{c}]");
            var paramNames = insertDict.Keys.Select(c => $"@{c}");
            string sql = $"INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)})";

            return await conn.ExecuteAsync(sql, new DynamicParameters(insertDict));
        }

        #endregion

        #region 3. 修改 (Update) - 使用 PhysicalTableName

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            var config = GetConfig(type);

            // ✅ 使用 PhysicalTableName
            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName，无法更新数据。");

            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();

            // 获取表结构
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            // 分拣数据
            var updateDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            // 先查旧数据的 JsonData (如果是局部更新，需要合并；如果是全量覆盖则不需要)
            // 这里假设是合并模式，或者简单起见直接覆盖 JsonData
            // 如果要完美支持，建议先 Select JsonData 出来反序列化，再 Merge，再 Update

            foreach (var kvp in data)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;
                if (kvp.Key == "ID") continue;

                if (dbColSet.Contains(kvp.Key))
                {
                    updateDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                else if (dbColSet.Contains("JsonData"))
                {
                    jsonDict[kvp.Key] = kvp.Value;
                }
            }

            // 处理 JsonData (简单策略：如果有新的扩展字段，就更新 JsonData 列)
            // ⚠️ 注意：这会覆盖旧的 JsonData。如果需要保留旧的扩展字段，需先读后写。
            // 鉴于您的前端通常提交全量表单，覆盖通常是可以接受的。
            if (jsonDict.Any() && dbColSet.Contains("JsonData"))
            {
                updateDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
            }

            // 自动填充
            if (dbColSet.Contains("UpdatedTime")) updateDict["UpdatedTime"] = DateTime.Now;
            if (dbColSet.Contains("UpdatedBy")) updateDict["UpdatedBy"] = "System";

            if (!updateDict.Any()) return 0;

            // 构建 SQL
            var setClauses = updateDict.Keys.Select(k => $"[{k}] = @{k}");
            string sql = $"UPDATE [{tableName}] SET {string.Join(", ", setClauses)} WHERE ID = @Id";

            var paramsDict = new DynamicParameters(updateDict);
            paramsDict.Add("Id", id);

            return await conn.ExecuteAsync(sql, paramsDict);
        }

        #endregion

        #region 4. 删除 (Delete) - 使用 PhysicalTableName

        public async Task<int> DeleteAsync(string type, int id)
        {
            var config = GetConfig(type);

            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName");

            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();

            // 物理删除
            string sql = $"DELETE FROM [{tableName}] WHERE ID = @Id";

            // 或者逻辑删除 (如果表里有 IsDeleted 字段)
            // string sql = $"UPDATE [{tableName}] SET IsDeleted = 1 WHERE ID = @Id";

            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        // 批量删除
        public async Task<int> BatchDeleteAsync(string type, List<int> ids)
        {
            if (ids == null || !ids.Any()) return 0;

            var config = GetConfig(type);
            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName");

            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();
            string sql = $"DELETE FROM [{tableName}] WHERE ID IN @Ids";

            return await conn.ExecuteAsync(sql, new { Ids = ids });
        }

        #endregion

        #region 辅助方法

        // 读取配置
        private DictItemConfig GetConfig(string type)
        {
            // 使用 Bind 将配置部分绑定到对象
            var config = _configuration.GetSection($"DictConfiguration:{type}").Get<DictItemConfig>();
            if (config == null)
            {
                throw new Exception($"未找到类型 '{type}' 的配置信息，请检查 dicts.json");
            }
            return config;
        }

        // 获取表结构 (缓存这步可以优化性能)
        private async Task<List<string>> GetTableSchemaAsync(IDbConnection conn, string tableName)
        {
            if (conn.State != ConnectionState.Open)
            {
                // Dapper 需要 open connection
                if (conn is System.Data.Common.DbConnection dbConn)
                    await dbConn.OpenAsync();
                else
                    conn.Open();
            }

            // 从系统视图读取列名
            var result = await conn.QueryAsync<string>(
                @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName",
                new { TableName = tableName });

            return result.ToList();
        }

        // JsonElement 转 C# 原生类型 (防止 Dapper 报错)
        private object DataToSqlValue(object val)
        {
            if (val is JsonElement je)
            {
                return je.ValueKind switch
                {
                    JsonValueKind.String => je.GetString(),
                    JsonValueKind.Number => je.GetDecimal(), // 兼容 int/decimal
                    JsonValueKind.True => 1,
                    JsonValueKind.False => 0,
                    JsonValueKind.Null => null,
                    _ => je.ToString()
                };
            }
            // 处理布尔值转 bit
            if (val is bool b) return b ? 1 : 0;

            return val;
        }

        #endregion
    }
}