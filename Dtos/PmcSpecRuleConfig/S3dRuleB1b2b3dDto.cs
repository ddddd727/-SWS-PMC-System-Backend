namespace PMCSystem_Backend.Models
{
    public class S3dRuleB1b2b3dDto
    {
        public int MaterialsCategoryCl { get; set; }
        public int GeometricIndustryStandardCl { get; set; }
        public int MaterialsGradeCl { get; set; }
        public int ScheduleThicknessCl { get; set; }
        public string RuleName { get; set; } = null!;
        public bool Status { get; set; }
    }

    public class S3dCodeB1b2b3dDto
    {
        public int MaterialsCategoryCl { get; set; }
        public int GeometricIndustryStandardCl { get; set; }
        public int MaterialsGradeCl { get; set; }
        public int ScheduleThicknessCl { get; set; }
        public string RuleName { get; set; } = null!;
        public bool Status { get; set; }
        public string? MaterialsCategoryCode { get; set; }
        public string? PipingStandardCode { get; set; }
        public string? MaterialsGradeCode { get; set; }
        public string? ScheduleThicknessCode { get; set; }
    }
}
