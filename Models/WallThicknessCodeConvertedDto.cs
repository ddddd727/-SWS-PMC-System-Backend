namespace PMCSystem_Backend.Models
{
    public class WallThicknessCodeConvertedDto
    {
        public int Id { get; set; }
        public decimal? Npd { get; set; }
        public string? Ndpunit { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public string? ScheduleThickness { get; set; }
        public int? EndStandardCl { get; set; }
        public string? EndStandard { get; set; }
        public decimal? PipingOutsideDiameter { get; set; }
        public decimal? WallThickness { get; set; }
        public bool? Status { get; set; }
    }
}
