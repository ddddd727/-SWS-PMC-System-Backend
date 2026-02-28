using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Implementations.DictStrategies;
using PMCSystem_Backend.Services.Interfaces;


namespace PMCSystem_Backend.Services.Implementations
{
    public class DictService : IDictService
    {
        private readonly PmcContext _context;
        private readonly DictConfigManager _configManager;
        private readonly DictStrategyFactory _strategyFactory;

        public DictService(PmcContext context,
            DictConfigManager configManager,
            DictStrategyFactory strategyFactory)
        {
            _context = context;
            _configManager = configManager;
            _strategyFactory = strategyFactory;
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
            result.Columns = config.Columns
     .Where(c => !c.DbField.Equals("JsonData", StringComparison.OrdinalIgnoreCase))
     .Select(c => new DictColumnDto
     {
         Prop = c.DbField,
         Label = c.Title,
         Show = !c.IsHidden,
         UiType = c.UiType ?? "Input",
         Required = c.IsRequired,
         IsPrimaryKey = c.IsPrimaryKey,
         IsReadOnly = c.IsReadOnly,

         // ✅ 映射 Options：把配置里的选项传给前端
         Options = c.Options,

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
                // 1. 如果有 JsonData，先把它“掏空”并合并到 row 中
                if (row.ContainsKey("JsonData") && row["JsonData"] is string jsonStr && !string.IsNullOrEmpty(jsonStr))
                {
                    try
                    {
                        var jsonObj = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
                        if (jsonObj != null)
                        {
                            foreach (var kvp in jsonObj)
                            {
                                // 只有当主表中没有这个字段时，才使用扩展字段，防止覆盖物理主键等
                                if (!row.ContainsKey(kvp.Key))
                                {
                                    // 💡 System.Text.Json 反序列化后的 Value 是 JsonElement
                                    // 这里建议做一个简单的拆箱，方便前端处理（可选）
                                    row[kvp.Key] = UnwrapJsonElement(kvp.Value);
                                }
                            }
                        }
                    }
                    catch { /* 忽略脏数据解析错误 */ }
                }

                // 2. 彻底移除 JsonData 字段，前端根本看不到它
                if (row.ContainsKey("JsonData"))
                {
                    row.Remove("JsonData");
                }
            }

            return result;
        }

        #endregion

        #region 2. 新增 (Add)

        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            var config = GetConfig(type);
            ValidateInput(config, data);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.AddAsync(type, config, data);
        }

        #endregion

        #region 3. 修改 (Update)

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            var config = GetConfig(type);
            ValidateInput(config, data);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.UpdateAsync(type, id, config, data);
        }

        #endregion



        #region 4. 删除 (Delete)

        public async Task<int> DeleteAsync(string type, int id)
        {
            var config = GetConfig(type);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.DeleteAsync(type, id, config);
        }

        #endregion

        #region 辅助方法

        private object UnwrapJsonElement(object val)
        {
            if (val is JsonElement je)
            {
                return je.ValueKind switch
                {
                    JsonValueKind.String => je.GetString(),
                    JsonValueKind.Number => je.GetDecimal(), // 或 GetDouble/GetInt32
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.Null => null,
                    _ => je.ToString()
                };
            }
            return val;
        }
        private DictItemConfig GetConfig(string type)
        {
            return _configManager.GetConfig(type);
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
        private void ValidateInput(DictItemConfig config, DictInputDto data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            var errors = new List<string>();

            foreach (var column in config.Columns)
            {
                // 只校验标记为必填 (IsRequired=true) 且非隐藏 (IsHidden=false) 的字段
                // 注意：有时候 ID 是 PK 但 IsHidden=false，需要排除 ID
                if (column.IsRequired && !column.IsPrimaryKey)
                {
                    // 检查 data 中是否包含该 Key，且值不为空
                    if (!data.TryGetValue(column.DbField, out var value) || IsNullOrEmpty(value))
                    {
                        errors.Add($"字段 '{column.Title}' ({column.DbField}) 不能为空");
                    }
                }
            }

            if (errors.Any())
            {
                // 抛出异常，Controller 会捕获并返回 400
                throw new Exception($"数据校验失败: {string.Join("; ", errors)}");
            }
        }

        // ✅ 新增：判空辅助方法 (兼容 null, 空字符串, JsonElement Null)
        private bool IsNullOrEmpty(object? value)
        {
            if (value == null) return true;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return true;

            // 处理 System.Text.Json 的 JsonElement
            if (value is JsonElement je)
            {
                return je.ValueKind == JsonValueKind.Null ||
                       je.ValueKind == JsonValueKind.Undefined ||
                       (je.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(je.GetString()));
            }

            return false;
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