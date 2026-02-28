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
            // 1. 获取配置与校验
            var config = GetConfig(type);
            ValidateInput(config, data);

            string tableName = config.PhysicalTableName;
            using var conn = _context.Database.GetDbConnection();

            // 2. 获取物理表 Schema
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            var insertDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            // 🌟 核心新增：准备接收前端的“填缝假 ID”
            bool hasExplicitId = false;
            int explicitId = 0;

            foreach (var kvp in data)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;

                // 🌟 拦截门槛：判断是否是前端传来的有效 ID
                if (kvp.Key.Equals("ID", StringComparison.OrdinalIgnoreCase))
                {
                    // 脱下 JSON 马甲，尝试解析为数字
                    var idVal = DataToSqlValue(kvp.Value);
                    if (idVal != null && int.TryParse(idVal.ToString(), out explicitId) && explicitId > 0)
                    {
                        // 只有大于 0 的正整数，才被认为是前端算好的填缝 ID，收下它！
                        insertDict["ID"] = explicitId;
                        hasExplicitId = true;
                    }
                    continue; // ID 处理完毕，直接跳过后面的逻辑
                }

                // 🚪 第一道门槛：查户口（只放行真正的物理列）
                if (dbColSet.Contains(kvp.Key))
                {
                    insertDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                // 🚪 第二道门槛：对暗号（放行 Ext_）
                else if (kvp.Key.StartsWith("Ext_", StringComparison.OrdinalIgnoreCase))
                {
                    jsonDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                // 🚪 第三道门槛：垃圾桶
                else
                {
                    continue;
                }
            }

            // 组装 JsonData
            if (jsonDict.Any() && dbColSet.Contains("JsonData"))
            {
                insertDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
            }

            // 自动填充系统字段
            if (dbColSet.Contains("CreatedTime")) insertDict["CreatedTime"] = DateTime.Now;
            if (dbColSet.Contains("CreatedBy")) insertDict["CreatedBy"] = "System";
            if (dbColSet.Contains("Status") && !insertDict.ContainsKey("Status")) insertDict["Status"] = 1;

            // 提取列名和参数名
            var colNames = insertDict.Keys.Select(c => $"[{c}]");
            var paramNames = insertDict.Keys.Select(c => $"@{c}");

            string sql = "";

            // 🌟 核心分流：决定怎么执行 SQL
            if (hasExplicitId)
            {
                // A 计划：前端传了算好的 3！
                // 开启 IDENTITY_INSERT 权限，把 3 强行塞进去，完事后再关闭权限。
                sql = $@"
            SET IDENTITY_INSERT [{tableName}] ON;
            INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)});
            SET IDENTITY_INSERT [{tableName}] OFF;
            SELECT {explicitId}; 
        ";
            }
            else
            {
                // B 计划：前端没传真 ID（防呆机制），交给数据库自己顺延
                sql = $"INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)}); SELECT CAST(SCOPE_IDENTITY() as int)";
            }

            try
            {
                // 执行插入
                return await conn.QuerySingleAsync<int>(sql, new DynamicParameters(insertDict));
            }
            catch (Exception ex)
            {
                // 🛡️ 终极防御：万一你的这张表根本没有设置 IDENTITY（自增）属性，开启 IDENTITY_INSERT 就会报错。
                // 如果触发了这个报错，我们直接退级成普通插入，依然能保证数据安全入库！
                if (ex.Message.Contains("does not have the identity property", StringComparison.OrdinalIgnoreCase))
                {
                    sql = $"INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)}); SELECT {explicitId};";
                    return await conn.QuerySingleAsync<int>(sql, new DynamicParameters(insertDict));
                }
                throw; // 其他真实的报错依然抛出
            }
        }

        #endregion

        #region 3. 修改 (Update)

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            // 1. 获取配置
            var config = GetConfig(type);

            // ✅ 新增：服务端必填项校验
            ValidateInput(config, data);

            if (string.IsNullOrEmpty(config.PhysicalTableName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 PhysicalTableName");

            string tableName = config.PhysicalTableName;

            using var conn = _context.Database.GetDbConnection();

            // ... (后续逻辑保持不变) ...
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);
            var definedColSet = config.Columns.Select(c => c.DbField).ToHashSet(StringComparer.OrdinalIgnoreCase);


            var updateDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            foreach (var kvp in data)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key)) continue;
                if (kvp.Key.Equals("ID", StringComparison.OrdinalIgnoreCase)) continue;

                // 🚪 第一道门槛：查户口（只放行真正的物理列）
                if (dbColSet.Contains(kvp.Key))
                {
                    // 比如：Status、PipingStandardCode 等真实存在于表里的字段
                    updateDict[kvp.Key] = DataToSqlValue(kvp.Value);
                    // 注意：AddAsync 里面这里是 insertDict
                }
                // 🚪 第二道门槛：对暗号（只放行以 Ext_ 开头的真扩展列）
                else if (kvp.Key.StartsWith("Ext_", StringComparison.OrdinalIgnoreCase))
                {
                    // 比如：前端新加的 Ext_4e9ef6e7，这是我们要存在 JsonData 里的
                    jsonDict[kvp.Key] = DataToSqlValue(kvp.Value);
                }
                // 🚪 第三道门槛：垃圾桶（所有不认识的字段，一律丢弃）
                else
                {
                    // 🚨 重点在这里！
                    // 像 GeometricIndustryPractice_Parent、PipingClass_Short 这种：
                    // 1. 不是物理列
                    // 2. 也不叫 Ext_xxx
                    // 它们走到这里，直接被 continue 跳过，绝对不可能再混进 jsonDict 里！
                    continue;
                }
            }

            // 处理 JsonData 字段的写入
            if (dbColSet.Contains("JsonData"))
            {
                // 只要有动态列数据就序列化，哪怕全被清空了，存个 "{}" 也好过不管
                if (jsonDict.Any())
                {
                    updateDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
                }
                else
                {
                    // 如果前端传过来的所有 Ext_ 值都被清空了，就存个空 JSON 对象
                    updateDict["JsonData"] = "{}";
                }
            }

            if (dbColSet.Contains("UpdatedTime")) updateDict["UpdatedTime"] = DateTime.Now;
            if (dbColSet.Contains("UpdatedBy")) updateDict["UpdatedBy"] = "System";

            if (!updateDict.Any()) return 0;

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