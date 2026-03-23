namespace PMCSystem_Backend.Modules.PMCRuleConfig.Dtos
{
    public class S3dRuleC1c2Dto
    {
        public int GeometricIndustryStandardCl { get; set; }
        public int PressureRatingCl { get; set; }
        public string RuleName { get; set; } = null!;
        public bool Status { get; set; }
    }

    public class S3dCodeC1c2Dto
    {
        public int GeometricIndustryStandardCl { get; set; }
        public int PressureRatingCl { get; set; }
        public string RuleName { get; set; } = null!;
        public bool Status { get; set; }
        public string? FlangeStandardCode { get; set; }
        public string? PressureRatingCode { get; set; }
    }
}
