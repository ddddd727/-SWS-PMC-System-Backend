using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;
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
            var config = GetConfig(type);

            if (string.IsNullOrEmpty(config.ViewName))
                throw new Exception($"配置错误：类型 '{type}' 缺少 ViewName，无法查询数据。");

            var result = new DictTableDto();

            // ── Meta（权限 / 分页 / 排序 / 行操作）──────────────────────────
            result.Meta = new DictTableMetaDto
            {
                Permissions = config.Permissions,
                Pagination = config.Pagination,
                DefaultSort = config.DefaultSort,
                RowActions = config.RowActions?.Select(a => new DictRowActionDto
                {
                    Key = a.Key,
                    Label = a.Label,
                    Icon = a.Icon,
                    Type = a.Type,
                    ConfirmText = a.ConfirmText,
                    VisibleWhen = MapConditionRule(a.VisibleWhen)
                }).ToList()
            };

            // ── Columns ──────────────────────────────────────────────────────
            result.Columns = config.Columns
                .Where(c => !c.DbField.Equals("JsonData", StringComparison.OrdinalIgnoreCase))
                .Select(c => new DictColumnDto
                {
                    Prop = c.DbField,
                    Label = c.Title,
                    Tooltip = c.Tooltip,
                    SortOrder = c.SortOrder,
                    Show = !c.IsHidden,
                    UiType = c.UiType ?? "Input",
                    Format = c.Format,
                    Required = c.IsRequired,
                    IsPrimaryKey = c.IsPrimaryKey,
                    IsReadOnly = c.IsReadOnly,
                    IsUnique = c.IsUnique,
                    IsSortable = c.IsSortable,
                    IsFilterable = c.IsFilterable,
                    DefaultValue = c.DefaultValue,
                    Validation = MapValidation(c.Validation),
                    CustomRules = c.CustomRules?.Select(r => new DictCustomRuleDto
                    {
                        Type = r.Type,
                        Url = r.Url,
                        Expression = r.Expression,
                        Trigger = r.Trigger,
                        Message = r.Message
                    }).ToList(),
                    VisibleWhen = MapConditionRule(c.VisibleWhen),
                    DataSource = MapDataSource(c.DataSource)
                }).ToList();

            // ── 按 SortOrder 排列列（没有填的排到最后）─────────────────────
            result.Columns = result.Columns
                .OrderBy(c => c.SortOrder ?? int.MaxValue)
                .ToList();

            // ── 查询数据 ──────────────────────────────────────────────────────
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var sql = new StringBuilder($"SELECT * FROM [{config.ViewName}] WHERE 1=1");
            var parameters = new DynamicParameters();

            // 关键字搜索（只对 Input 类型未隐藏的列）
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var conditions = config.Columns
                    .Where(c => (c.UiType == "Input" || c.UiType == null) && !c.IsHidden)
                    .Select(c => $"[{c.DbField}] LIKE @Kw")
                    .ToList();

                if (conditions.Any())
                {
                    sql.Append(" AND ( ").Append(string.Join(" OR ", conditions)).Append(" )");
                    parameters.Add("Kw", $"%{keyword}%");
                }
            }

            // 排序：优先读 DefaultSort 配置，兜底 ID DESC
            if (config.DefaultSort != null)
            {
                var order = config.DefaultSort.Order.ToUpper() == "ASC" ? "ASC" : "DESC";
                sql.Append($" ORDER BY [{config.DefaultSort.Field}] {order}");
            }
            else if (config.Columns.Any(c => c.DbField.Equals("ID", StringComparison.OrdinalIgnoreCase)))
            {
                sql.Append(" ORDER BY ID DESC");
            }

            var rows = await conn.QueryAsync(sql.ToString(), parameters);

            result.Rows = rows
                .Select(row => (IDictionary<string, object>)row)
                .Select(d => new Dictionary<string, object>(d))
                .ToList();

            // JsonData 展平
            foreach (var row in result.Rows)
            {
                if (row.TryGetValue("JsonData", out var jsonRaw) && jsonRaw is string jsonStr && !string.IsNullOrEmpty(jsonStr))
                {
                    try
                    {
                        var jsonObj = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
                        if (jsonObj != null)
                            foreach (var kvp in jsonObj)
                                if (!row.ContainsKey(kvp.Key))
                                    row[kvp.Key] = UnwrapJsonElement(kvp.Value);
                    }
                    catch { /* 忽略脏数据 */ }
                }
                row.Remove("JsonData");
            }

            return result;
        }

        #endregion

        #region 2. 新增 (Add)

        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            var config = GetConfig(type);
            await ValidateInputAsync(config, data);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.AddAsync(type, config, data);
        }

        #endregion

        #region 3. 修改 (Update)

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            var config = GetConfig(type);
            await ValidateInputAsync(config, data);
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

        #region 5. 校验 (Validate - 供 CustomRules.Type=Url 使用)

        public async Task<DictValidateResponse> ValidateFieldAsync(string type, DictValidateRequest request)
        {
            var config = GetConfig(type);
            var column = config.Columns.FirstOrDefault(c =>
                c.DbField.Equals(request.Field, StringComparison.OrdinalIgnoreCase));

            if (column == null)
                return new DictValidateResponse { Valid = true };

            // 1. 内置规则校验
            var builtinError = ValidateBuiltinRules(column, request.Value);
            if (builtinError != null)
                return new DictValidateResponse { Valid = false, Message = builtinError };

            // 2. IsUnique 单列唯一校验（查数据库）
            if (column.IsUnique && !string.IsNullOrEmpty(config.PhysicalTableName))
            {
                var uniqueError = await ValidateUniqueAsync(config.PhysicalTableName, column.DbField, request.Value, request.Row);
                if (uniqueError != null)
                    return new DictValidateResponse { Valid = false, Message = uniqueError };
            }

            // 3. UniqueConstraints 联合唯一校验
            if (config.UniqueConstraints != null && request.Row != null)
            {
                var constraintError = await ValidateUniqueConstraintsAsync(config, request.Row);
                if (constraintError != null)
                    return new DictValidateResponse { Valid = false, Message = constraintError };
            }

            return new DictValidateResponse { Valid = true };
        }

        #endregion

        #region 6. 获取选项 (GetOptions)

        public async Task<IEnumerable<dynamic>> GetCodeListOptionsAsync(string tableName, DataSourceRelationConfig? relation = null)
        {
            // 第一步：找子表ID
            var childTable = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.CodeListTableName == tableName);

            if (childTable == null) return Enumerable.Empty<dynamic>();

            // 第二步：查子表全量数据
            var childValues = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.CodeListTableId == childTable.Id)
                .OrderBy(v => v.CodeListNumber)
                .ToListAsync();

            // 没配 LoadRelation → 直接返回，字段名用表名前缀
            if (relation == null)
            {
                return childValues.Select(v => (dynamic)new Dictionary<string, object?>
                {
                    [$"{tableName}_CL"] = v.CodeListNumber,
                    [$"{tableName}_Short"] = v.ShortStringValue,
                    [$"{tableName}_Long"] = v.LongStringValue,
                }).ToList();
            }

            // 配了 LoadRelation → 渐进查父表

            // 第三步：用 SQL 查 S3D_Common_CodeListHierarchy 找父表ID
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            var parentTableId = await conn.ExecuteScalarAsync<int?>(
                "SELECT ParentCodeListTableID FROM S3D_Common_CodeListHierarchy WHERE CodeListTableID = @id",
                new { id = childTable.Id }
            );

            if (parentTableId == null)
            {
                // 找不到父表 → 降级返回，不报错
                return childValues.Select(v => (dynamic)new Dictionary<string, object?>
                {
                    [$"{tableName}_CL"] = v.CodeListNumber,
                    [$"{tableName}_Short"] = v.ShortStringValue,
                    [$"{tableName}_Long"] = v.LongStringValue,
                }).ToList();
            }

            // 第四步：只查子节点实际用到的父节点
            // CodeListTableID + CodeListNumber 联合定位，避免跨表 Number 重复
            var neededParentNumbers = childValues
                .Where(c => c.ParentCodeListNumber != null)
                .Select(c => c.ParentCodeListNumber!.Value)
                .Distinct()
                .ToList();

            var parentValues = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.CodeListTableId == parentTableId
                         && neededParentNumbers.Contains(v.CodeListNumber))
                .ToListAsync();

            // 第五步：内存关联组装，字段名全部用表名前缀
            return childValues.Select(v =>
            {
                var parent = v.ParentCodeListNumber == null ? null
                    : parentValues.FirstOrDefault(
                        p => p.CodeListTableId == parentTableId
                          && p.CodeListNumber == v.ParentCodeListNumber
                    );

                return (dynamic)new Dictionary<string, object?>
                {
                    [$"{tableName}_CL"] = v.CodeListNumber,
                    [$"{tableName}_Short"] = v.ShortStringValue,
                    [$"{tableName}_Long"] = v.LongStringValue,
                    [$"{tableName}_Parent_Long"] = parent?.LongStringValue
                };
            }).ToList();
        }

        #endregion

        #region 私有辅助方法

        private DictItemConfig GetConfig(string type) => _configManager.GetConfig(type);

        /// <summary>完整校验（新增 / 修改时调用）</summary>
        private async Task ValidateInputAsync(DictItemConfig config, DictInputDto data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            var errors = new List<string>();

            foreach (var column in config.Columns)
            {
                if (column.IsPrimaryKey) continue;

                data.TryGetValue(column.DbField, out var value);

                // IsRequired
                if (column.IsRequired && IsNullOrEmpty(value))
                {
                    errors.Add($"字段 '{column.Title}' ({column.DbField}) 不能为空");
                    continue; // 为空则跳过后续规则
                }

                if (IsNullOrEmpty(value)) continue;

                // 内置 Validation 规则
                var builtinError = ValidateBuiltinRules(column, value);
                if (builtinError != null) errors.Add(builtinError);
            }

            // UniqueConstraints 联合唯一（后端兜底）
            if (!errors.Any() && config.UniqueConstraints != null && !string.IsNullOrEmpty(config.PhysicalTableName))
            {
                var row = data.ToDictionary(k => k.Key, v => v.Value);
                var constraintError = await ValidateUniqueConstraintsAsync(config, row);
                if (constraintError != null) errors.Add(constraintError);
            }

            if (errors.Any())
                throw new Exception($"数据校验失败: {string.Join("; ", errors)}");
        }

        /// <summary>内置规则校验（Min/Max/MinLength/MaxLength/Pattern）</summary>
        private string? ValidateBuiltinRules(DictColumnConfig column, object? value)
        {
            var v = column.Validation;
            if (v == null || value == null) return null;

            var message = v.CustomMessage;

            var strVal = value?.ToString() ?? "";

            if (v.MinLength.HasValue && strVal.Length < v.MinLength.Value)
                return message ?? $"'{column.Title}' 最少 {v.MinLength} 个字符";

            if (v.MaxLength.HasValue && strVal.Length > v.MaxLength.Value)
                return message ?? $"'{column.Title}' 最多 {v.MaxLength} 个字符";

            if ((v.Min.HasValue || v.Max.HasValue) && double.TryParse(strVal, out var num))
            {
                if (v.Min.HasValue && num < v.Min.Value)
                    return message ?? $"'{column.Title}' 不能小于 {v.Min}";
                if (v.Max.HasValue && num > v.Max.Value)
                    return message ?? $"'{column.Title}' 不能大于 {v.Max}";
            }

            if (!string.IsNullOrEmpty(v.Pattern) && !Regex.IsMatch(strVal, v.Pattern))
                return message ?? $"'{column.Title}' 格式不正确";

            return null;
        }

        /// <summary>单列唯一校验</summary>
        private async Task<string?> ValidateUniqueAsync(string tableName, string field, object? value, Dictionary<string, object>? row)
        {
            var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var sql = $"SELECT COUNT(1) FROM [{tableName}] WHERE [{field}] = @Value";
            var parameters = new DynamicParameters();
            parameters.Add("Value", value?.ToString());

            // 编辑时排除自身（通过 ID）
            if (row != null && row.TryGetValue("ID", out var idObj) && idObj != null)
            {
                sql += " AND ID != @Id";
                parameters.Add("Id", idObj.ToString());
            }

            var count = await conn.ExecuteScalarAsync<int>(sql, parameters);
            return count > 0 ? $"字段值已存在，请勿重复" : null;
        }

        /// <summary>联合唯一约束校验</summary>
        private async Task<string?> ValidateUniqueConstraintsAsync(DictItemConfig config, Dictionary<string, object> row)
        {
            if (config.UniqueConstraints == null || string.IsNullOrEmpty(config.PhysicalTableName))
                return null;

            var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            foreach (var group in config.UniqueConstraints)
            {
                if (!group.All(f => row.ContainsKey(f))) continue;

                var conditions = group.Select((f, i) => $"[{f}] = @p{i}").ToList();
                var sql = $"SELECT COUNT(1) FROM [{config.PhysicalTableName}] WHERE {string.Join(" AND ", conditions)}";
                var parameters = new DynamicParameters();
                for (int i = 0; i < group.Count; i++)
                    parameters.Add($"p{i}", row[group[i]]?.ToString());

                // 编辑时排除自身
                if (row.TryGetValue("ID", out var idObj) && idObj != null)
                {
                    sql += " AND ID != @Id";
                    parameters.Add("Id", idObj.ToString());
                }

                var count = await conn.ExecuteScalarAsync<int>(sql, parameters);
                if (count > 0)
                {
                    var fieldTitles = group.Select(f =>
                        config.Columns.FirstOrDefault(c => c.DbField == f)?.Title ?? f
                    );
                    return $"【{string.Join(" + ", fieldTitles)}】组合已存在相同记录，请检查后重试";
                }
            }

            return null;
        }

        private static ConditionRuleDto? MapConditionRule(ConditionRule? rule)
        {
            if (rule == null) return null;
            return new ConditionRuleDto
            {
                Field = rule.Field,
                Operator = rule.Operator,
                Value = rule.Value
            };
        }

        private static DictValidationDto? MapValidation(DictValidationConfig? v)
        {
            if (v == null) return null;
            return new DictValidationDto
            {
                Min = v.Min,
                Max = v.Max,
                MinLength = v.MinLength,
                MaxLength = v.MaxLength,
                Pattern = v.Pattern,
                CustomMessage = v.CustomMessage
            };
        }

        private static DictDataSourceDto? MapDataSource(DictDataSourceConfig? ds)
        {
            if (ds == null) return null;
            return new DictDataSourceDto
            {
                Url = ds.Url,
                LabelField = ds.LabelField,
                ValueField = ds.ValueField,
                Options = ds.Options?.Select(o => MapStaticOption(o)).ToList(),
                DependsOn = ds.DependsOn?.Select(d => new DataSourceDependencyDto
                {
                    Field = d.Field,
                    ParamName = d.ParamName
                }).ToList(),
                ValueMapping = ds.ValueMapping,
                FilterUsed = ds.FilterUsed,
                LoadRelation = ds.LoadRelation == null ? null : new DataSourceRelationDto
                {
                    Direction = ds.LoadRelation.Direction,
                    MappedField = ds.LoadRelation.MappedField
                }
            };
        }

        private static DictStaticOptionDto MapStaticOption(DictStaticOption o) => new()
        {
            Label = o.Label,
            Value = o.Value,
            Disabled = o.Disabled,
            Children = o.Children?.Select(MapStaticOption).ToList()
        };

        private static object UnwrapJsonElement(object val)
        {
            if (val is JsonElement je)
            {
                return je.ValueKind switch
                {
                    JsonValueKind.String => je.GetString()!,
                    JsonValueKind.Number => je.GetDecimal(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.Null => null!,
                    _ => je.ToString()
                };
            }
            return val;
        }

        private static bool IsNullOrEmpty(object? value)
        {
            if (value == null) return true;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return true;
            if (value is JsonElement je)
                return je.ValueKind == JsonValueKind.Null ||
                       je.ValueKind == JsonValueKind.Undefined ||
                       (je.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(je.GetString()));
            return false;
        }

        #endregion
    }
}