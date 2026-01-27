namespace PMCSystem_Backend.Dtos.Dict
{
    public class DictTableDto
    {
        public List<DictColumnDto> Columns { get; set; } = new();
        public IEnumerable<dynamic> Rows { get; set; }
    }

    public class DictColumnDto
    {
        public string Prop { get; set; }
        public string Label { get; set; }
        public bool Show { get; set; }
        public string UiType { get; set; }
        public bool Required { get; set; }
        public string DataSource { get; set; }
    }
}