namespace PMCSystem_Backend.Models
{
    public class S3dRulePipingBendParameterDto
    {
        public int Id { get; set; }
        public int MaterialsCategoryCl { get; set; }
        public decimal NormalDiameter { get; set; }
        public string UnitType { get; set; } = null!;
        public int ScheduleThicknessCl { get; set; }
        public decimal BendRadiusMultiplier { get; set; }
        public bool Status { get; set; }
        public string? JsonData { get; set; }
    }

    public class CreateS3dRulePipingBendParameterDto
    {
        public int? MaterialsCategoryCl { get; set; }
        public decimal? NormalDiameter { get; set; }
        public string? UnitType { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public decimal? BendRadiusMultiplier { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateS3dRulePipingBendParameterDto : CreateS3dRulePipingBendParameterDto
    {
        public int Id { get; set; }
    }
}

