namespace PMCSystem_Backend.Modules.DesignRules.Entities;

public partial class S3dCodePlainPipingGenericData
{
    public int Id { get; set; }

    public double? NominalPipingDiameter { get; set; }

    public string? NominalDiameterUnits { get; set; }

    public int? EndStandardCl { get; set; }

    public string? EndStandard { get; set; }

    public int? ScheduleThicknessCl { get; set; }

    public string? ScheduleThickness { get; set; }

    public string? PipingOutsideDiameter { get; set; }

    public string? WallThickness { get; set; }

    public bool? Status { get; set; }
}
