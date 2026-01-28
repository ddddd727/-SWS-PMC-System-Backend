namespace PMCSystem_Backend.Dtos.Dict
{
    // 对应 appsettings.json 的根节点
    public class RootDictConfig
    {
        public Dictionary<string, DictItemConfig> DictConfiguration { get; set; } = new();
    }

    // 对应每个具体的字典配置（例如 "UserDict", "RoleDict"）
    public class DictItemConfig
    {
        public string TableName { get; set; } = string.Empty;
        public List<DictColumnConfig> Columns { get; set; } = new();
    }

    // 对应每一列的详细配置
    public class DictColumnConfig
    {
        public string DbField { get; set; } = string.Empty; // 数据库字段名
        public string Title { get; set; } = string.Empty;   // 前端显示的标题
        public bool IsHidden { get; set; }                  // 是否在列表隐藏
        public string? UiType { get; set; }                 // 控件类型 (Input, Select, etc.)
        public bool IsRequired { get; set; }                // 是否必填
        public bool IsPrimaryKey { get; set; }              // 是否是主键
        public string? DataSource { get; set; }             // 下拉框的数据源标识
        public bool IsReadOnly { get; set; }                // 是否只读 (之前缺少的属性)
    }
}