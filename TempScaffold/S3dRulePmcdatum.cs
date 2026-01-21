using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePmcdatum
{
    public int Id { get; set; }

    public string ShipType { get; set; } = null!;

    public string ShipNo { get; set; } = null!;

    public string Pmccode { get; set; } = null!;

    public string PipingClassName { get; set; } = null!;

    public string MaterialsCategoryName { get; set; } = null!;

    public string PipingStandardName { get; set; } = null!;

    public string MaterialsGradeName { get; set; } = null!;

    public string FlangeStandardName { get; set; } = null!;

    public string PressureRatingName { get; set; } = null!;

    public string ScheduleThicknessName { get; set; } = null!;

    public string? PipeStandard { get; set; }

    public string? ElbowStandard { get; set; }

    public string? RedStandard { get; set; }

    public string? TeeStandard { get; set; }

    public string? SleeveStandard { get; set; }

    public string? BossesStandard { get; set; }

    public string? SaddlesStandard { get; set; }

    public string? CapsStandard { get; set; }

    public string? OverpassStandard { get; set; }

    public string? AccessoriesStandard { get; set; }

    public string? FlangeStandard { get; set; }

    public string? BlindFlangeStandard { get; set; }

    public string? GasketStandard { get; set; }

    public string? BoltStandard { get; set; }

    public string? NutStandard { get; set; }

    public string? WasherStandard { get; set; }

    public string? Status { get; set; }

    public string? JsonData { get; set; }
}
