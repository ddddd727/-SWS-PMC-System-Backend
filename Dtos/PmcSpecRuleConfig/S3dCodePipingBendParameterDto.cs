namespace PMCSystem_Backend.Models
{
    public class S3dCodePipingBendParameterDto
    {
        public int Id { get; set; }
        public int? MaterialsCategoryCl { get; set; }
        public string? MaterialsCategory { get; set; }
        public decimal? NormalDiameter { get; set; }
        public string? UnitType { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public string? ScheduleThickness { get; set; }
        public decimal? BendRadiusMultiplier { get; set; }
        public bool? Status { get; set; }
    }
}
