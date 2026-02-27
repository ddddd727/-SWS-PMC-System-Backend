namespace PMCSystem_Backend.Dtos.PmcSpecRuleConfig
{
    public class S3dDictPipingComponentTypeDto
    {
        public int Id { get; set; }
        public string ComponentTypeName { get; set; } = null!;
        public string ComponentTypeDescription { get; set; } = null!;
        public bool Status { get; set; }
    }
}
