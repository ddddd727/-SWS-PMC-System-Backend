namespace PMCSystem_Backend.Models
{
    public class S3dRuleShortCodeHierarchyRuleDto
    {
        public int Id { get; set; }
        public string ShortCodeHierarchyType { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
    }

    public class CreateS3dRuleShortCodeHierarchyRuleDto
    {
        public string ShortCodeHierarchyType { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
    }

    public class UpdateS3dRuleShortCodeHierarchyRuleDto : CreateS3dRuleShortCodeHierarchyRuleDto
    {
        public int Id { get; set; }
    }
}
