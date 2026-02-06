using System.Collections.Generic;

namespace PMCSystem_Backend.Dtos.Dict
{
    // 整个表格的数据结构
    public class DictTableDto
    {
        public List<DictColumnDto> Columns { get; set; } = new();
        public List<Dictionary<string, object>> Rows { get; set; } = new();
    }

    // 列定义 (发送给前端)
    public class DictColumnDto
    {
        public string Prop { get; set; } = string.Empty;    // 对应前端的 prop
        public string Label { get; set; } = string.Empty;   // 对应前端的 label
        public bool Show { get; set; }                      // 是否显示
        public string UiType { get; set; } = "Input";       // 控件类型
        public bool Required { get; set; }                  // 是否必填
        public bool IsPrimaryKey { get; set; }              // 是否主键
        public bool IsReadOnly { get; set; }                // 是否只读

        // ✅ 修复：改为对象类型
        public DictDataSourceDto? DataSource { get; set; }
    }

    // ✅ 新增：对应前端需要的下拉配置结构
    public class DictDataSourceDto
    {
        public string Url { get; set; } = string.Empty;
        public string LabelField { get; set; } = string.Empty;
        public string ValueField { get; set; } = string.Empty;
        public Dictionary<string, string>? ValueMapping { get; set; }
    }
}