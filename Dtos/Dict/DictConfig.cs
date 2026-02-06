using System.Collections.Generic;

namespace PMCSystem_Backend.Dtos.Dict
{
    // 对应 appsettings.json 的根节点
    public class RootDictConfig
    {
        public Dictionary<string, DictItemConfig> DictConfiguration { get; set; } = new();
    }

    // 对应每个具体的字典配置（例如 "std-series"）
    public class DictItemConfig
    {
        public string DisplayName { get; set; } = string.Empty;
        public string ViewName { get; set; } = string.Empty;
        public string PhysicalTableName { get; set; } = string.Empty;
        public string CodeListTableName { get; set; } = string.Empty;
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
        public bool IsReadOnly { get; set; }                // 是否只读

        // ✅ 修复：改为对象类型，匹配 dicts.json 中的结构
        public DictDataSourceConfig? DataSource { get; set; }
    }

    // ✅ 新增：下拉源配置类
    public class DictDataSourceConfig
    {
        public string Url { get; set; } = string.Empty;
        public string LabelField { get; set; } = string.Empty;
        public string ValueField { get; set; } = string.Empty;
        public Dictionary<string, string>? ValueMapping { get; set; }
    }
}