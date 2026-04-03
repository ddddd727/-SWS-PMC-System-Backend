namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dRuleShortCodeMapDto
    {
        public int Id { get; set; }
        public int ComponentTypeId { get; set; }
        public string ShortCode { get; set; } = null!;
    }

    public class CreateS3dRuleShortCodeMapDto
    {
        public int ComponentTypeId { get; set; }
        public string ShortCode { get; set; } = null!;
    }

    public class UpdateS3dRuleShortCodeMapDto
    {
        public int Id { get; set; }
        public int ComponentTypeId { get; set; }
        public string ShortCode { get; set; } = null!;
    }
}
