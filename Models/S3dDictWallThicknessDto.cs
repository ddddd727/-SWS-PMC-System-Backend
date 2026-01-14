namespace PMCSystem_Backend.Models
{
    public class S3dDictWallThicknessDto
    {
        public int Id { get; set; }
        public decimal Npd { get; set; }
        public string Ndpunit { get; set; } = null!;
        public int ScheduleThicknessCl { get; set; }
        public int EndStandardCl { get; set; }
        public decimal PipingOutsideDiameter { get; set; }
        public decimal WallThickness { get; set; }
        public bool Status { get; set; }
        public string? JsonData { get; set; }
    }

    public class CreateS3dDictWallThicknessDto
    {
        public decimal? Npd { get; set; }
        public string? Ndpunit { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public int? EndStandardCl { get; set; }
        public decimal? PipingOutsideDiameter { get; set; }
        public decimal? WallThickness { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateS3dDictWallThicknessDto : CreateS3dDictWallThicknessDto
    {
        public int Id { get; set; }
    }
}
