using System.Collections.Generic;

namespace PMCSystem_Backend.Dtos.Dict
{
    // 整个表格的数据结构（返回给前端）
    public class DictTableDto
    {
        public DictTableMetaDto Meta { get; set; } = new();
        public List<DictColumnDto> Columns { get; set; } = new();
        public List<Dictionary<string, object>> Rows { get; set; } = new();
    }

    // 表格级元数据（权限、分页、排序、行操作）
    public class DictTableMetaDto
    {
        public DictPermissionsConfig? Permissions { get; set; }
        public DictPaginationConfig? Pagination { get; set; }
        public DictSortConfig? DefaultSort { get; set; }
        public List<DictRowActionDto>? RowActions { get; set; }
    }

    // 行操作按钮（透传给前端）
    public class DictRowActionDto
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string Type { get; set; } = "default";
        public string? ConfirmText { get; set; }
        public ConditionRuleDto? VisibleWhen { get; set; }
    }

    // 列定义（发送给前端）
    public class DictColumnDto
    {
        public string Prop { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;

        public string? Tooltip { get; set; }
        public int? SortOrder { get; set; }
        public string UiType { get; set; } = "Input";
        public string? Format { get; set; }

        public bool Show { get; set; } = true;
        public bool Required { get; set; } = false;
        public bool IsPrimaryKey { get; set; } = false;
        public bool IsReadOnly { get; set; } = false;
        public bool IsUnique { get; set; } = false;
        public bool IsSortable { get; set; } = false;
        public bool IsFilterable { get; set; } = false;

        public object? DefaultValue { get; set; }

        public DictValidationDto? Validation { get; set; }
        public List<DictCustomRuleDto>? CustomRules { get; set; }
        public ConditionRuleDto? VisibleWhen { get; set; }
        public DictDataSourceDto? DataSource { get; set; }
    }

    // 内置校验规则 DTO
    public class DictValidationDto
    {
        public double? Min { get; set; }
        public double? Max { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string? Pattern { get; set; }
        public string? CustomMessage { get; set; }
    }

    // 自定义校验规则 DTO
    public class DictCustomRuleDto
    {
        public string Type { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Expression { get; set; }
        public string Trigger { get; set; } = "onSubmit";
        public string Message { get; set; } = string.Empty;
    }

    // 条件规则 DTO（VisibleWhen / RowAction.VisibleWhen）
    public class ConditionRuleDto
    {
        public string Field { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public object? Value { get; set; }
    }

    // DataSource DTO
    public class DictDataSourceDto
    {
        public string? Url { get; set; }
        public string LabelField { get; set; } = string.Empty;
        public string ValueField { get; set; } = string.Empty;
        public List<DictStaticOptionDto>? Options { get; set; }
        public List<DataSourceDependencyDto>? DependsOn { get; set; }
        public Dictionary<string, string>? ValueMapping { get; set; }
        public DataSourceRelationDto? LoadRelation { get; set; }
        public bool FilterUsed { get; set; } = true;
    }

    public class DataSourceRelationDto
    {
        /// <summary>Parent / Children</summary>
        public string Direction { get; set; } = "Parent";

        /// <summary>挂到 option 上的字段名，前端 ValueMapping 用这个名字取值</summary>
        public string MappedField { get; set; } = string.Empty;
    }

    public class DictStaticOptionDto
    {
        public string Label { get; set; } = string.Empty;
        public object? Value { get; set; }
        public bool Disabled { get; set; } = false;
        public List<DictStaticOptionDto>? Children { get; set; }
    }

    public class DataSourceDependencyDto
    {
        public string Field { get; set; } = string.Empty;
        public string ParamName { get; set; } = string.Empty;
    }

    // 远程校验请求体（CustomRules.Type=Url 时，前端 POST 此结构）
    public class DictValidateRequest
    {
        public string Field { get; set; } = string.Empty;
        public object? Value { get; set; }
        public Dictionary<string, object>? Row { get; set; }
    }

    // 远程校验统一响应体
    public class DictValidateResponse
    {
        public bool Valid { get; set; }
        public string? Message { get; set; }
    }
}