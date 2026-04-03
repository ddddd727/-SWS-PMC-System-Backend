namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.CodelistTable
{
    /// <summary>
    /// Codelist项内容
    /// </summary>
    public class CodeListItem
    {
        // 主键ID
        public int Id { get; set; }

        // Codelist值
        public int CodeListNumber { get; set; }

        // 短描述
        public string ShortStringValue { get; set; }
        
        // 长描述
        public string LongStringValue { get; set; }

        // 父级Codelist值（可为空）
        public int? ParentCodeListNumber { get; set; }

        // 是否为用户自定义项
        public bool IsUserDefine { get; set; }

        // 是否启用
        public bool Status { get; set; }
    }
}
