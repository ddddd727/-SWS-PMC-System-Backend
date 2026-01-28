namespace PMCSystem_Backend.Dtos.Dict
{
    // 整个表格的数据结构：包含表头定义(Columns)和数据行(Rows)
    public class DictTableDto
    {
        public List<DictColumnDto> Columns { get; set; } = new();
        public List<Dictionary<string, object>> Rows { get; set; } = new();
    }

    // 发送给前端的列定义（也就是前端根据这个来生成表头和表单）
    public class DictColumnDto
    {
        public string Prop { get; set; } = string.Empty;    // 对应前端的 prop
        public string Label { get; set; } = string.Empty;   // 对应前端的 label
        public bool Show { get; set; }                      // 是否显示
        public string UiType { get; set; } = "Input";       // 控件类型
        public bool Required { get; set; }                  // 是否必填
        public bool IsPrimaryKey { get; set; }              // 是否主键
        public string? DataSource { get; set; }             // 下拉源
        public bool IsReadOnly { get; set; }                // 是否只读
    }
}