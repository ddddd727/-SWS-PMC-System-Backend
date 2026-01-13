namespace PMCSystem_Backend.Entities
{
    public partial class S3dDictWallThicknessCodeConverted
    {
        public int Id { get; set; }
        public decimal Npd { get; set; }
        public string Ndpunit { get; set; } = null!;
        public int ScheduleThicknessCl { get; set; }
        public string? ScheduleThickness { get; set; }
        public int EndStandardCl { get; set; }
        public string? EndStandard { get; set; }
        public decimal PipingOutsideDiameter { get; set; }
        public decimal WallThickness { get; set; }
        public bool Status { get; set; }
    }
}
