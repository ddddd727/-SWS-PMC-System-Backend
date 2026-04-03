namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dCommonPlainPipingGenericDataDto
    {
        public int Id { get; set; }
        public double? NominalPipingDiameter { get; set; }
        public string? NominalDiameterUnits { get; set; }
        public int? EndStandardCl { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public string? PipingOutsideDiameter { get; set; }
        public string? WallThickness { get; set; }
        public bool? Status { get; set; }
    }

    public class CreateS3dCommonPlainPipingGenericDataDto
    {
        public double? NominalPipingDiameter { get; set; }
        public string? NominalDiameterUnits { get; set; }
        public int? EndStandardCl { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public string? PipingOutsideDiameter { get; set; }
        public string? WallThickness { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateS3dCommonPlainPipingGenericDataDto : CreateS3dCommonPlainPipingGenericDataDto
    {
        public int Id { get; set; }
    }
}
