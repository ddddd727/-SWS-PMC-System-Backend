namespace PMCSystem_Backend.Models
{
    public class PipeLimitRuleDto
    {
        public int Id { get; set; }
        public string? RuleName { get; set; }
        public string? PipingClassCode { get; set; }
        public string? PipingStandardCode { get; set; }
        public string? MaterialsGradeCode { get; set; }
        public string? PressureRatingCode { get; set; }
    }
}
