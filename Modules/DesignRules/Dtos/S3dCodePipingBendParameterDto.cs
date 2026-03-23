namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dCodePipingBendParameterDto
    {
        public int Id { get; set; }
        public int MaterialsCategoryCl { get; set; }
        public string MaterialsCategory { get; set; } = null!;
        public int? GeometricIndustryStandardCl { get; set; }
        public string? GeometricIndustryStandard { get; set; }
        public int? MaterialsGradeCl { get; set; }
        public string? MaterialsGrade { get; set; }
        public double NormalDiameter { get; set; }
        public string UnitType { get; set; } = null!;
        public string? WallThicknessFrom { get; set; }
        public string? WallThicknessTo { get; set; }
        public decimal BendRadiusMultiplier { get; set; }
        public bool Status { get; set; }
        public string? JsonData { get; set; }
    }
}
