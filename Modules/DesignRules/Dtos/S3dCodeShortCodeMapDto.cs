namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dCodeShortCodeMapDto
    {
        public int Id { get; set; }
        public int ComponentTypeId { get; set; }
        public string ComponentTypeName { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
    }
}
