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
        private readonly AppDbContext _context;
        private readonly DictConfigManager _configManager;
        private readonly DictStrategyFactory _strategyFactory;

        public DictService(AppDbContext context,
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
                    // 需要 LoadRelation 时，补齐父节点信息，供前端 ValueMapping 使用
                    [$"{tableName}_Parent_CL"] = parent?.CodeListNumber,
                    [$"{tableName}_Parent_Short"] = parent?.ShortStringValue,
                    [$"{tableName}_Parent_Long"] = parent?.LongStringValue
                };
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<dynamic>> GetDropdownOptionsAsync(string dictType, string? field = null, string? optionsSource = null)
        {
            var config = GetConfig(dictType);
            var column = ResolveOptionsColumn(config, dictType, field);
            var ds = column?.DataSource;

            string source;
            if (!string.IsNullOrWhiteSpace(optionsSource))
            {
                var o = optionsSource.Trim();
                if (o.Equals(DictOptionsSourceKind.View, StringComparison.OrdinalIgnoreCase))
                    source = DictOptionsSourceKind.View;
                else if (o.Equals(DictOptionsSourceKind.CodeList, StringComparison.OrdinalIgnoreCase))
                    source = DictOptionsSourceKind.CodeList;
                else
                {
                    throw new InvalidOperationException(
                        $"不支持的 source 参数: {optionsSource}，请使用 {DictOptionsSourceKind.View} 或 {DictOptionsSourceKind.CodeList}。");
                }
            }
            else
            {
                var sourceRaw = ds?.OptionsSource ?? config.OptionsSource;
                if (string.IsNullOrWhiteSpace(sourceRaw))
                {
                    var hasViewHint = !string.IsNullOrEmpty(ds?.OptionsViewName)
                                      || !string.IsNullOrEmpty(config.OptionsViewName)
                                      || !string.IsNullOrEmpty(ds?.OptionsRefDictType)
                                      || (ds?.OptionsViewColumns?.Count ?? 0) > 0
                                      || (config.OptionsViewColumns?.Count ?? 0) > 0;
                    source = hasViewHint ? DictOptionsSourceKind.View : DictOptionsSourceKind.CodeList;
                }
                else
                {
                    source = sourceRaw.Trim();
                }
            }

            if (source.Equals(DictOptionsSourceKind.View, StringComparison.OrdinalIgnoreCase))
            {
                var viewName = ResolveOptionsViewName(config, ds);
                var cols = ds?.OptionsViewColumns ?? config.OptionsViewColumns;
                if (cols == null || cols.Count == 0)
                    cols = InferViewColumnsFromDataSource(ds);

                if (string.IsNullOrEmpty(viewName) || cols == null || cols.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"字典 '{dictType}' 使用 View 策略时须配置：本字典的 ViewName（或 OptionsViewName / OptionsRefDictType），以及 OptionsViewColumns 或由列 DataSource 的 LabelField/ValueField/ValueMapping 推断列。");
                }

                return await GetDictViewOptionsAsync(viewName, cols);
            }

            if (string.IsNullOrEmpty(config.CodeListTableName))
            {
                throw new InvalidOperationException(
                    $"字典 '{dictType}' 未配置 CodeListTableName（CodeList 策略），或请改为 OptionsSource=View 并配置视图列。");
            }

            var relation = ds?.LoadRelation
                           ?? config.Columns.FirstOrDefault(c => c.DataSource?.LoadRelation != null)
                               ?.DataSource?.LoadRelation;

            return await GetCodeListOptionsAsync(config.CodeListTableName, relation);
        }

        // 显式接口实现：避免由于可空引用类型/可选参数默认值等差异导致的编译器匹配失败。
        Task<IEnumerable<dynamic>> IDictService.GetDropdownOptionsAsync(string dictType, string? field, string? optionsSource)
            => GetDropdownOptionsAsync(dictType, field, optionsSource);

        /// <inheritdoc />
        public async Task<IEnumerable<dynamic>> GetDictViewOptionsAsync(string viewName, IReadOnlyList<string> columns)
        {
            if (string.IsNullOrWhiteSpace(viewName) || columns == null || columns.Count == 0)
                return Enumerable.Empty<dynamic>();

            foreach (var col in columns)
            {
                if (string.IsNullOrEmpty(col) || !Regex.IsMatch(col, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
                    throw new ArgumentException($"非法列名: {col}");
            }

            var distinctCols = string.Join(", ", columns.Select(c => $"[{c}]"));
            var orderCol = columns[0];
            var sql = $"SELECT DISTINCT {distinctCols} FROM [{viewName}] ORDER BY [{orderCol}]";

            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            var rows = await conn.QueryAsync(sql);
            var result = new List<dynamic>();
            foreach (var row in rows)
            {
                if (row is not IDictionary<string, object> dict)
                    continue;

                var mapped = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                foreach (var kv in dict)
                    mapped[kv.Key] = kv.Value is DBNull ? null : kv.Value;

                result.Add(mapped);
            }

            return result;
        }

        /// <summary>
        /// 列级 OptionsViewName 优先；否则通过 <see cref="DictDataSourceConfig.OptionsRefDictType"/> 读取目标字典的 <see cref="DictItemConfig.ViewName"/>。
        /// </summary>
        private string? ResolveOptionsViewName(DictItemConfig config, DictDataSourceConfig? ds)
        {
            var direct = ds?.OptionsViewName ?? config.OptionsViewName;
            if (!string.IsNullOrWhiteSpace(direct))
                return direct;

            var refType = ds?.OptionsRefDictType;
            if (!string.IsNullOrWhiteSpace(refType))
            {
                var refConfig = _configManager.GetConfig(refType);
                if (string.IsNullOrWhiteSpace(refConfig.ViewName))
                {
                    throw new InvalidOperationException(
                        $"字典 '{refType}' 未配置 ViewName，无法作为 OptionsRefDictType 的视图来源。");
                }

                return refConfig.ViewName;
            }

            // 未配 OptionsViewName / OptionsRefDictType 时，与本字典 JSON 中的列表 ViewName 一致（无需重复写）
            return string.IsNullOrWhiteSpace(config.ViewName) ? null : config.ViewName;
        }

        /// <summary>
        /// 未显式配置 OptionsViewColumns 时，用 ValueField、LabelField、ValueMapping 的值拼 DISTINCT 列（顺序与下拉回填一致）。
        /// </summary>
        private static List<string>? InferViewColumnsFromDataSource(DictDataSourceConfig? ds)
        {
            if (ds == null) return null;

            var order = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void Add(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return;
                if (seen.Add(s)) order.Add(s);
            }

            Add(ds.ValueField);
            Add(ds.LabelField);
            if (ds.ValueMapping != null)
            {
                foreach (var v in ds.ValueMapping.Values)
                    Add(v);
            }

            return order.Count > 0 ? order : null;
        }

        /// <summary>
        /// 定位当前请求对应的 Select 列：优先 field；否则 Url 自引用 /api/dict/options/{type}；否则第一个带 Url 的 Select。
        /// </summary>
        private static DictColumnConfig? ResolveOptionsColumn(DictItemConfig config, string dictType, string? field)
        {
            static bool IsSelectLike(string? ui) =>
                ui is "Select" or "MultiSelect" or "TreeSelect";

            var candidates = config.Columns
                .Where(c => IsSelectLike(c.UiType) && c.DataSource != null)
                .ToList();

            if (candidates.Count == 0)
                return null;

            if (!string.IsNullOrWhiteSpace(field))
            {
                return candidates.FirstOrDefault(c =>
                    c.DbField.Equals(field, StringComparison.OrdinalIgnoreCase));
            }

            var needle = $"/options/{dictType}";
            foreach (var c in candidates)
            {
                var url = c.DataSource!.Url;
                if (string.IsNullOrEmpty(url))
                    continue;
                if (url.Contains(needle, StringComparison.OrdinalIgnoreCase))
                    return c;
            }

            return candidates[0];
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
                if (builtinError != null)
                {
                    errors.Add(builtinError);
                    continue;
                }

                // IsUnique 单列唯一（与列配置一致，保存时兜底）
                if (column.IsUnique && !string.IsNullOrEmpty(config.PhysicalTableName))
                {
                    var uniqueError = await ValidateUniqueAsync(config.PhysicalTableName, column.DbField, value, data);
                    if (uniqueError != null) errors.Add(uniqueError);
                }
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

            var physicalCols = await GetObjectColumnsAsync(conn, config.PhysicalTableName);
            HashSet<string>? viewCols = null;

            foreach (var group in config.UniqueConstraints)
            {
                if (!group.All(f => row.ContainsKey(f))) continue;

                // 优先用实表校验；当字段不在实表时，退回到视图校验（满足“用视图字段做联合校验”的业务语义）
                var missingInPhysical = group.Any(f => !physicalCols.Contains(f));
                var fromObject = config.PhysicalTableName;
                HashSet<string> fromCols = physicalCols;

                if (missingInPhysical)
                {
                    if (string.IsNullOrWhiteSpace(config.ViewName))
                        throw new Exception(
                            $"联合唯一校验失败：字段 {string.Join(", ", group)} 在物理表 '{config.PhysicalTableName}' 中不存在，且未配置 ViewName 无法降级校验。");

                    viewCols ??= await GetObjectColumnsAsync(conn, config.ViewName);

                    var hasAllInView = group.All(f => viewCols.Contains(f));
                    if (hasAllInView)
                    {
                        fromObject = config.ViewName!;
                        fromCols = viewCols;
                    }
                }

                var conditions = group.Select((f, i) => $"[{f}] = @p{i}").ToList();
                var sql = $"SELECT COUNT(1) FROM [{fromObject}] WHERE {string.Join(" AND ", conditions)}";
                var parameters = new DynamicParameters();
                for (int i = 0; i < group.Count; i++)
                    parameters.Add($"p{i}", row[group[i]]?.ToString());

                // 编辑时排除自身
                if (row.TryGetValue("ID", out var idObj) && idObj != null)
                {
                    // 只有目标对象上存在 ID 列时，才能做“排除自身”的编辑校验
                    if (fromCols.Contains("ID"))
                    {
                        sql += " AND [ID] != @Id";
                        parameters.Add("Id", idObj.ToString());
                    }
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

        /// <summary>
        /// 获取数据库对象（表/视图）的列名集合，用于在“校验字段来自视图但实表缺列”时做降级。
        /// </summary>
        private static async Task<HashSet<string>> GetObjectColumnsAsync(System.Data.Common.DbConnection conn, string objectName)
        {
            static (string? schema, string name) SplitSchema(string fullName)
            {
                var trimmed = fullName.Trim();
                var parts = trimmed.Split('.', StringSplitOptions.RemoveEmptyEntries);
                return parts.Length == 2 ? (parts[0], parts[1]) : (null, trimmed);
            }

            var (schema, name) = SplitSchema(objectName);

            // 兼容 TABLE / VIEW：如果传入的是视图名，在信息架构里也能取到对应列
            var sql = @"
SELECT c.COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS c
JOIN INFORMATION_SCHEMA.TABLES t
  ON c.TABLE_NAME = t.TABLE_NAME
 AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
WHERE c.TABLE_NAME = @Name
  AND (@Schema IS NULL OR c.TABLE_SCHEMA = @Schema)
  AND (t.TABLE_TYPE = 'BASE TABLE' OR t.TABLE_TYPE = 'VIEW');";

            var cols = await conn.QueryAsync<string>(sql, new { Name = name, Schema = schema });
            return new HashSet<string>(cols, StringComparer.OrdinalIgnoreCase);
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
                },
                OptionsSource = ds.OptionsSource,
                OptionsViewName = ds.OptionsViewName,
                OptionsViewColumns = ds.OptionsViewColumns,
                OptionsRefDictType = ds.OptionsRefDictType
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