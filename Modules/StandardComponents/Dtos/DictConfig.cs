using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.StandardComponents.Dtos
{
    /// <summary>
    /// 下拉选项加载策略，用于 <see cref="DictItemConfig"/> / <see cref="DictDataSourceConfig"/> 的 OptionsSource 字段。
    /// </summary>
    public static class DictOptionsSourceKind
    {
        public const string CodeList = "CodeList";
        public const string View = "View";
    }

    // 对应每个具体的字典配置（例如 "std-series"）
    public class DictItemConfig
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? ViewName { get; set; }
        public string? PhysicalTableName { get; set; }
        public string? CodeListTableName { get; set; }

        /// <summary>
        /// 表格级默认选项策略（列 DataSource 未指定时兜底）。CodeList=读 CodeList 表；View=读视图 DISTINCT（未配 OptionsViewName 时用本字典的 <see cref="ViewName"/>）。
        /// </summary>
        public string? OptionsSource { get; set; }

        /// <summary>OptionsSource=View 时使用的视图名；不填则与 <see cref="ViewName"/> 相同。</summary>
        public string? OptionsViewName { get; set; }

        /// <summary>OptionsSource=View 时 DISTINCT 的列（表格级兜底）。</summary>
        public List<string>? OptionsViewColumns { get; set; }

        public string? HandlerType { get; set; }

        /// <summary>联合唯一约束，每个子数组代表一组必须唯一的字段组合</summary>
        public List<List<string>>? UniqueConstraints { get; set; }

        public DictPermissionsConfig? Permissions { get; set; }
        public DictPaginationConfig? Pagination { get; set; }
        public DictSortConfig? DefaultSort { get; set; }
        public List<DictRowActionConfig>? RowActions { get; set; }

        public List<DictColumnConfig> Columns { get; set; } = new();
    }

    // ── 表格级配置 ────────────────────────────────────────────

    public class DictPermissionsConfig
    {
        public bool AllowCreate { get; set; } = true;
        public bool AllowEdit { get; set; } = true;
        public bool AllowDelete { get; set; } = true;
        public bool AllowExport { get; set; } = false;
        public bool AllowImport { get; set; } = false;
    }

    public class DictPaginationConfig
    {
        public int DefaultPageSize { get; set; } = 20;
        public List<int>? PageSizeOptions { get; set; }
    }

    public class DictSortConfig
    {
        public string Field { get; set; } = string.Empty;
        /// <summary>asc / desc</summary>
        public string Order { get; set; } = "desc";
    }

    public class DictRowActionConfig
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Icon { get; set; }
        /// <summary>default / primary / danger / warning</summary>
        public string Type { get; set; } = "default";
        public string? ConfirmText { get; set; }
        public ConditionRule? VisibleWhen { get; set; }
    }

    // ── 列级配置 ──────────────────────────────────────────────

    public class DictColumnConfig
    {
        public string DbField { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public string? Tooltip { get; set; }
        public int? SortOrder { get; set; }

        /// <summary>Input / Textarea / Number / Switch / Select / MultiSelect /
        /// TreeSelect / DatePicker / DateTimePicker / Upload / JsonInput</summary>
        public string? UiType { get; set; }

        /// <summary>显示格式：日期 "YYYY-MM-DD"，数字 "#,##0.00"</summary>
        public string? Format { get; set; }

        public bool IsPrimaryKey { get; set; } = false;
        public bool IsHidden { get; set; } = false;
        public bool IsReadOnly { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool IsUnique { get; set; } = false;
        public bool IsSortable { get; set; } = false;
        public bool IsFilterable { get; set; } = false;

        /// <summary>新增行时的默认值</summary>
        public object? DefaultValue { get; set; }

        public DictValidationConfig? Validation { get; set; }
        public List<DictCustomRule>? CustomRules { get; set; }
        public ConditionRule? VisibleWhen { get; set; }
        public DictDataSourceConfig? DataSource { get; set; }
    }

    // ── 校验规则 ──────────────────────────────────────────────

    public class DictValidationConfig
    {
        public double? Min { get; set; }
        public double? Max { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        /// <summary>正则表达式</summary>
        public string? Pattern { get; set; }
        public string? CustomMessage { get; set; }
    }

    public class DictCustomRule
    {
        /// <summary>Url = 远程接口校验；Expression = 前端 JS 表达式校验</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>Type=Url 时有效，POST {field, value, row}，返回 {valid, message?}</summary>
        public string? Url { get; set; }

        /// <summary>Type=Expression 时有效，可访问 value / row，返回 true 表示通过</summary>
        public string? Expression { get; set; }

        /// <summary>onChange / onBlur / onSubmit，默认 onSubmit</summary>
        public string Trigger { get; set; } = "onSubmit";

        public string Message { get; set; } = string.Empty;
    }

    // ── 通用条件规则（VisibleWhen / RowActions.VisibleWhen 复用）──

    public class ConditionRule
    {
        public string Field { get; set; } = string.Empty;
        /// <summary>eq / neq / in / notIn / gt / gte / lt / lte / contains</summary>
        public string Operator { get; set; } = string.Empty;
        public object? Value { get; set; }
    }

    // ── DataSource ────────────────────────────────────────────

    public class DictDataSourceConfig
    {
        /// <summary>动态数据源接口（与 Options 二选一）。可对同一 dict options 追加 ?source=view 或 ?source=codelist 覆盖策略。</summary>
        public string? Url { get; set; }

        /// <summary>静态选项列表（与 Url 二选一）</summary>
        public List<DictStaticOption>? Options { get; set; }

        public string LabelField { get; set; } = string.Empty;
        public string ValueField { get; set; } = string.Empty;

        /// <summary>级联依赖：依赖字段变化时重新请求 Url</summary>
        public List<DataSourceDependency>? DependsOn { get; set; }

        /// <summary>选中后回填到行其他字段的映射，key=目标DbField，value=option字段名</summary>
        public Dictionary<string, string>? ValueMapping { get; set; }

        /// <summary>选中后需要自动回填到当前行的字段列表（字段名需与视图返回字段一致）</summary>
        public List<string>? AutoFillFields { get; set; }

        /// <summary>父子级关联加载配置（可选），配置后查询时自动带出父级或子级信息</summary>
        public DataSourceRelationConfig? LoadRelation { get; set; }

        /// <summary>是否过滤已被其他行使用的选项，默认 true；设为 false 时始终显示全量选项</summary>
        public bool FilterUsed { get; set; } = true;

        /// <summary>
        /// 本列下拉的选项策略，优先于表格级 <see cref="DictItemConfig.OptionsSource"/>。
        /// 便于同一字典内不同字段分别来自 CodeList 或不同物理视图。
        /// </summary>
        public string? OptionsSource { get; set; }

        /// <summary>OptionsSource=View 时使用的视图名（列级可覆盖表格级）。</summary>
        public string? OptionsViewName { get; set; }

        /// <summary>OptionsSource=View 时 DISTINCT 的列名（列级可覆盖表格级）。</summary>
        public List<string>? OptionsViewColumns { get; set; }

        /// <summary>
        /// OptionsSource=View 且未配 <see cref="OptionsViewName"/> 时，从该字典 type 的配置读取 <see cref="DictItemConfig.ViewName"/>（如复用 mat-category 已声明的视图）。
        /// </summary>
        public string? OptionsRefDictType { get; set; }
    }

    public class DataSourceRelationConfig
    {
        /// <summary>加载方向：Parent=带出父级信息 / Children=带出子级列表</summary>
        public string Direction { get; set; } = "Parent";

        /// <summary>加载出来挂到每条 option 上的字段名，ValueMapping 用这个名字取值</summary>
        public string MappedField { get; set; } = string.Empty;
    }

    public class DictStaticOption
    {
        public string Label { get; set; } = string.Empty;
        public object? Value { get; set; }
        public bool Disabled { get; set; } = false;
        public List<DictStaticOption>? Children { get; set; }
    }

    public class DataSourceDependency
    {
        /// <summary>依赖的同行字段名（DbField）</summary>
        public string Field { get; set; } = string.Empty;
        /// <summary>传入 Url 请求时的参数名</summary>
        public string ParamName { get; set; } = string.Empty;
    }
}