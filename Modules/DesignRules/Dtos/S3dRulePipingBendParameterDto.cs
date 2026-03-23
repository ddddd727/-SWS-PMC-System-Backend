namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dRulePipingBendParameterDto
    {
        public int Id { get; set; }
        public int MaterialsCategoryCl { get; set; }
        public int? GeometricIndustryStandardCl { get; set; }
        public int? MaterialsGradeCl { get; set; }
        public double NormalDiameter { get; set; }
        public string UnitType { get; set; } = null!;
        public string? WallThicknessFrom { get; set; }
        public string? WallThicknessTo { get; set; }
        public decimal BendRadiusMultiplier { get; set; }
        public bool Status { get; set; }
        public string? JsonData { get; set; }
    }

    public class CreateS3dRulePipingBendParameterDto
    {
        public int? MaterialsCategoryCl { get; set; }
        public int? GeometricIndustryStandardCl { get; set; }
        public int? MaterialsGradeCl { get; set; }
        public double? NormalDiameter { get; set; }
        public string? UnitType { get; set; }
        public string? WallThicknessFrom { get; set; }
        public string? WallThicknessTo { get; set; }
        public decimal? BendRadiusMultiplier { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateS3dRulePipingBendParameterDto : CreateS3dRulePipingBendParameterDto
    {
        public int Id { get; set; }
    }
}
