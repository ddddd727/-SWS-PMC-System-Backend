using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations.DictStrategies
{
    public class AttributeDictStrategy : IDictStrategy
    {
        private readonly PmcContext _context;
        public AttributeDictStrategy(PmcContext context) => _context = context;

        public async Task<int> AddAsync(string type, DictItemConfig config, DictInputDto data)
        {
            string tableName = config.PhysicalTableName;
            using var conn = _context.Database.GetDbConnection();
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            var insertDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();
            bool hasExplicitId = false;
            int explicitId = 0;

            foreach (var kvp in data)
            {
                if (kvp.Key.Equals("ID", StringComparison.OrdinalIgnoreCase))
                {
                    var idVal = DataToSqlValue(kvp.Value);
                    if (idVal != null && int.TryParse(idVal.ToString(), out explicitId) && explicitId > 0)
                    {
                        insertDict["ID"] = explicitId;
                        hasExplicitId = true;
                    }
                    continue;
                }
                if (dbColSet.Contains(kvp.Key)) insertDict[kvp.Key] = DataToSqlValue(kvp.Value);
                else if (kvp.Key.StartsWith("Ext_", StringComparison.OrdinalIgnoreCase)) jsonDict[kvp.Key] = DataToSqlValue(kvp.Value);
            }

            if (jsonDict.Any() && dbColSet.Contains("JsonData")) insertDict["JsonData"] = JsonSerializer.Serialize(jsonDict);
            if (dbColSet.Contains("CreatedTime")) insertDict["CreatedTime"] = DateTime.Now;
            if (dbColSet.Contains("CreatedBy")) insertDict["CreatedBy"] = "System";

            var colNames = insertDict.Keys.Select(c => $"[{c}]");
            var paramNames = insertDict.Keys.Select(c => $"@{c}");

            string sql = hasExplicitId
                ? $"SET IDENTITY_INSERT [{tableName}] ON; INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)}); SET IDENTITY_INSERT [{tableName}] OFF; SELECT {explicitId};"
                : $"INSERT INTO [{tableName}] ({string.Join(",", colNames)}) VALUES ({string.Join(",", paramNames)}); SELECT CAST(SCOPE_IDENTITY() as int)";

            return await conn.QuerySingleAsync<int>(sql, new DynamicParameters(insertDict));
        }

        public async Task<int> UpdateAsync(string type, int id, DictItemConfig config, DictInputDto data)
        {
            string tableName = config.PhysicalTableName;
            using var conn = _context.Database.GetDbConnection();
            var dbColumns = await GetTableSchemaAsync(conn, tableName);
            var dbColSet = new HashSet<string>(dbColumns, StringComparer.OrdinalIgnoreCase);

            var updateDict = new Dictionary<string, object>();
            var jsonDict = new Dictionary<string, object>();

            foreach (var kvp in data)
            {
                if (kvp.Key.Equals("ID", StringComparison.OrdinalIgnoreCase)) continue;
                if (dbColSet.Contains(kvp.Key)) updateDict[kvp.Key] = DataToSqlValue(kvp.Value);
                else if (kvp.Key.StartsWith("Ext_", StringComparison.OrdinalIgnoreCase)) jsonDict[kvp.Key] = DataToSqlValue(kvp.Value);
            }

            if (dbColSet.Contains("JsonData")) updateDict["JsonData"] = jsonDict.Any() ? JsonSerializer.Serialize(jsonDict) : "{}";
            if (dbColSet.Contains("UpdatedTime")) updateDict["UpdatedTime"] = DateTime.Now;
            if (dbColSet.Contains("UpdatedBy")) updateDict["UpdatedBy"] = "System";

            var setClauses = updateDict.Keys.Select(k => $"[{k}] = @{k}");
            string sql = $"UPDATE [{tableName}] SET {string.Join(", ", setClauses)} WHERE ID = @Id";
            var paramsDict = new DynamicParameters(updateDict);
            paramsDict.Add("Id", id);
            await conn.ExecuteAsync(sql, paramsDict);
            return id;
        }

        public async Task<int> DeleteAsync(string type, int id, DictItemConfig config)
        {
            using var conn = _context.Database.GetDbConnection();
            return await conn.ExecuteAsync($"DELETE FROM [{config.PhysicalTableName}] WHERE ID = @Id", new { Id = id });
        }

        private object DataToSqlValue(object val)
        {
            if (val is JsonElement je) return je.ValueKind switch
            {
                JsonValueKind.String => je.GetString(),
                JsonValueKind.Number => je.GetDecimal(),
                JsonValueKind.True => 1,
                JsonValueKind.False => 0,
                JsonValueKind.Null => null,
                _ => je.ToString()
            };
            return val is bool b ? (b ? 1 : 0) : val;
        }

        private async Task<List<string>> GetTableSchemaAsync(IDbConnection conn, string tableName)
        {
            var result = await conn.QueryAsync<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName", new { TableName = tableName });
            return result.ToList();
        }
    }
}