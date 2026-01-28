namespace PMCSystem_Backend.Models
{
    public class S3dRuleAb2b3c2Dto
    {
        public int PipingClassCl { get; set; }
        public int GeometricIndustryStandardCl { get; set; }
        public int MaterialsGradeCl { get; set; }
        public int PressureRatingCl { get; set; }
        public string? RuleName { get; set; }
        public bool Status { get; set; }
    }

    public class S3dCodeAb2b3c2Dto
    {
        public int PipingClassCl { get; set; }
        public int GeometricIndustryStandardCl { get; set; }
        public int MaterialsGradeCl { get; set; }
        public int PressureRatingCl { get; set; }
        public string? RuleName { get; set; }
        public bool Status { get; set; }
        public string? PipingClassCode { get; set; }
        public string? PipingStandardCode { get; set; }
        public string? MaterialsGradeCode { get; set; }
        public string? PressureRatingCode { get; set; }
    }
}
