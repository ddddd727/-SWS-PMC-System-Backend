using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using System.Data;
using System.Data.Common; // 添加此引用以支持 DbDataReader
using System.Text.Json;

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

        private DictItemConfig GetConfig(string type)
        {
            var root = _configuration.Get<RootDictConfig>();
            if (root?.DictConfiguration == null || !root.DictConfiguration.TryGetValue(type, out var config))
                throw new Exception($"未找到字典配置: {type}");
            return config;
        }

        // 辅助：获取物理表结构 (仅用于写入时的校验)
        // 🛠️ 修复：改用原生 ADO.NET，解决 Dapper 不支持 CommandBehavior 参数的问题
        private async Task<List<string>> GetTableSchemaAsync(IDbConnection conn, string tableName)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            // 使用原生 Command 以支持 SchemaOnly
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM [{tableName}]";

            // ExecuteReaderAsync 在这里需要转为 DbCommand 才能使用 CommandBehavior 的异步重载，
            // 或者直接使用同步 ExecuteReader (SchemaOnly 极快，同步通常无影响)，
            // 为了稳妥，这里使用同步 ExecuteReader 配合 Task.Run 或者直接用 CommandBehavior
            // 注意：Dapper 的 conn.ExecuteReaderAsync 不接受 CommandBehavior。

            using var reader = await Task.Run(() => cmd.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo));

            var columns = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++) columns.Add(reader.GetName(i));
            return columns;
        }

        // 辅助：视图名 -> 物理表名
        private string GetPhysicalTableName(string configTableName)
        {
            if (configTableName.StartsWith("View_", StringComparison.OrdinalIgnoreCase))
                return configTableName.Replace("View_", "S3D_", StringComparison.OrdinalIgnoreCase);
            return configTableName;
        }

        // ==========================================================
        // 🟢 查 (Read)
        // ==========================================================
        // 🛠️ 修复：添加 keyword 参数以匹配 IDictService 接口 (CS0535 错误)
        public async Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null)
        {
            var config = GetConfig(type);
            var viewName = config.TableName;

            // 1. 准备前端列定义
            var frontendColumns = new List<DictColumnDto>();
            var allowedFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (config.Columns != null)
            {
                foreach (var colConfig in config.Columns)
                {
                    allowedFields.Add(colConfig.DbField);
                    frontendColumns.Add(new DictColumnDto
                    {
                        Prop = colConfig.DbField,
                        Label = colConfig.Title,
                        Show = !colConfig.IsHidden,
                        UiType = colConfig.UiType ?? "Input",
                        Required = colConfig.IsRequired,
                        IsPrimaryKey = colConfig.IsPrimaryKey,
                        DataSource = colConfig.DataSource,
                        IsReadOnly = colConfig.IsReadOnly // 🛠️ 这里现在可以正常通过编译了
                    });
                }
            }

            using var conn = _context.Database.GetDbConnection();

            // 2. 查全量数据
            // 注意：如果 keyword 有值，建议在这里拼接 SQL WHERE 或者在内存中过滤
            // 简单起见，这里保持原有逻辑，你可以在后续添加 keyword 的过滤逻辑
            string sql = $"SELECT * FROM [{viewName}]";
            var rawRows = await conn.QueryAsync(sql);

            // 3. 数据清洗
            var filteredRows = new List<Dictionary<string, object>>();

            foreach (var row in rawRows)
            {
                var rawDict = (IDictionary<string, object>)row;
                var newDict = new Dictionary<string, object>();

                // 简单的内存关键字过滤示例 (可选)
                bool matchKeyword = string.IsNullOrWhiteSpace(keyword);

                foreach (var field in allowedFields)
                {
                    var dbKey = rawDict.Keys.FirstOrDefault(k => k.Equals(field, StringComparison.OrdinalIgnoreCase));
                    var val = dbKey != null ? rawDict[dbKey] : null;
                    newDict[field] = val;

                    // 如果有关键字，简单检查是否有列包含该值
                    if (!matchKeyword && val != null && val.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        matchKeyword = true;
                    }
                }

                if (matchKeyword)
                {
                    filteredRows.Add(newDict);
                }
            }

            return new DictTableDto { Columns = frontendColumns, Rows = filteredRows };
        }

        // ==========================================================
        // 🟡 增/改/删 (保持不变)
        // ==========================================================
        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            var config = GetConfig(type);
            var physicalTable = GetPhysicalTableName(config.TableName);

            using var conn = _context.Database.GetDbConnection();
            var dbColumns = await GetTableSchemaAsync(conn, physicalTable);

            var validCols = dbColumns
                .Where(c => !c.Equals("Id", StringComparison.OrdinalIgnoreCase) &&
                            data.Keys.Any(k => k.Equals(c, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!validCols.Any()) throw new Exception("没有有效的插入字段");

            var colNames = validCols.Select(c => $"[{c}]");
            var paramNames = validCols.Select(c => $"@{c}");
            string sql = $"INSERT INTO [{physicalTable}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)})";

            var parameters = new DynamicParameters();
            foreach (var col in validCols)
            {
                var key = data.Keys.First(k => k.Equals(col, StringComparison.OrdinalIgnoreCase));
                parameters.Add(col, DataToSqlValue(data[key]));
            }

            return await conn.ExecuteAsync(sql, parameters);
        }

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            var config = GetConfig(type);
            var physicalTable = GetPhysicalTableName(config.TableName);

            using var conn = _context.Database.GetDbConnection();
            var pkCol = config.Columns?.FirstOrDefault(c => c.IsPrimaryKey)?.DbField ?? "Id";
            var dbColumns = await GetTableSchemaAsync(conn, physicalTable);

            var validCols = dbColumns
                .Where(c => !c.Equals(pkCol, StringComparison.OrdinalIgnoreCase) &&
                            data.Keys.Any(k => k.Equals(c, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!validCols.Any()) throw new Exception("没有需要更新的字段");

            var setClause = string.Join(", ", validCols.Select(c => $"[{c}] = @{c}"));
            string sql = $"UPDATE [{physicalTable}] SET {setClause} WHERE [{pkCol}] = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            foreach (var col in validCols)
            {
                var key = data.Keys.First(k => k.Equals(col, StringComparison.OrdinalIgnoreCase));
                parameters.Add(col, DataToSqlValue(data[key]));
            }

            return await conn.ExecuteAsync(sql, parameters);
        }

        public async Task<int> DeleteAsync(string type, int id)
        {
            var config = GetConfig(type);
            var physicalTable = GetPhysicalTableName(config.TableName);
            var pkCol = config.Columns?.FirstOrDefault(c => c.IsPrimaryKey)?.DbField ?? "Id";

            string sql = $"DELETE FROM [{physicalTable}] WHERE [{pkCol}] = @Id";
            using var conn = _context.Database.GetDbConnection();
            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        private object DataToSqlValue(object val)
        {
            if (val is JsonElement je) return je.ToString();
            return val;
        }
    }
}