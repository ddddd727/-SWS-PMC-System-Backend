namespace PMCSystem_Backend.Dtos.CodelistTable
{
    /// <summary>
    /// CodeList表信息
    /// </summary>
    public class CodeListTableInfo
    {
        // 主键ID
        public int Id { get; set; }

        // CodeList表名
        public string TableName { get; set; }

        // 是否为用户自定义表
        public bool IsUserDefined { get; set; }

        // 相关专业
        public string Major { get; set; }

        // 包含的CodeList项
        public List<CodeListItem> Items { get; set; } = new();
    }
}
