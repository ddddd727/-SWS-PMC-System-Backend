namespace PMCSystem_Backend.Modules.PMCRuleConfig.Dtos
{
    public class MainMaterialRuleDto
    {
        public int Id { get; set; }
        public string? RuleName { get; set; }
        public string? MaterialsCategoryCode { get; set; }
        public string? PipingStandardCode { get; set; }
        public string? MaterialsGradeCode { get; set; }
        public string? ScheduleThicknessCode { get; set; }
    }
}
