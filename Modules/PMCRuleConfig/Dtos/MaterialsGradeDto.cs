namespace PMCSystem_Backend.Modules.PMCRuleConfig.Dtos
{
    public class MaterialsGradeDto
    {
        public string MaterialsGradeCode { get; set; } = null!;
        public string MaterialsGradeDesc { get; set; } = null!;
        public int MaterialsGradeCl { get; set; }
    }
}
