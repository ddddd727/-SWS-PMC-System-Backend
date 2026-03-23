namespace PMCSystem_Backend.Modules.PMCRuleConfig.Dtos
{
    public class FlangeRuleDto
    {
        public int Id { get; set; }
        public string? RuleName { get; set; }
        public string? FlangeStandardCode { get; set; }
        public string? PressureRatingCode { get; set; }
    }
}
