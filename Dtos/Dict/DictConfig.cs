namespace PMCSystem_Backend.Dtos.Dict
{
    // ⬇️ 命名空间改为了 PMCSystem_Backend.Dtos.Dict
    // 这样只有引用了这个命名空间的地方才能看到它，不会干扰全局

    public class RootDictConfig
    {
        public Dictionary<string, string> CommonColumnMapping { get; set; }
        public Dictionary<string, DictItemConfig> DictConfiguration { get; set; }
    }

    public class DictItemConfig
    {
        public string TableName { get; set; }
        public List<DictColumnConfig> Columns { get; set; }
    }

    public class DictColumnConfig
    {
        public string DbField { get; set; }
        public string Title { get; set; }
        public bool IsHidden { get; set; }
        public bool IsRequired { get; set; }
        public string UiType { get; set; }
        public string DataSource { get; set; }
    }
}