using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

public partial class S3dRulePmcData
{
    public int Id { get; set; }

    public string ShipType { get; set; } = null!;

    public string ShipNo { get; set; } = null!;

    public string Pmccode { get; set; } = null!;

    public string PipingClassName { get; set; } = null!;

    public string MaterialsCategoryName { get; set; } = null!;

    public string PipingStandardName { get; set; } = null!;

    public string? MaterialsGradeName { get; set; } = null!;

    public string? FlangeStandardName { get; set; } = null!;

    public string? PressureRatingName { get; set; } = null!;

    public string? ScheduleThicknessName { get; set; } = null!;

    public List<PmcStandardInfo>? PipeStandard { get; set; }

    public List<PmcStandardInfo>? ElbowStandard { get; set; }

    public List<PmcStandardInfo>? RedStandard { get; set; }

    public List<PmcStandardInfo>? TeeStandard { get; set; }

    public List<PmcStandardInfo>? SleeveStandard { get; set; }

    public List<PmcStandardInfo>? BossesStandard { get; set; }

    public List<PmcStandardInfo>? SaddlesStandard { get; set; }

    public List<PmcStandardInfo>? CapsStandard { get; set; }

    public List<PmcStandardInfo>? OverpassStandard { get; set; }

    public List<PmcStandardInfo>? AccessoriesStandard { get; set; }

    public List<PmcStandardInfo>? FlangeStandard { get; set; }

    public List<PmcStandardInfo>? BlindFlangeStandard { get; set; }

    public List<PmcStandardInfo>? GasketStandard { get; set; }

    public List<PmcStandardInfo>? BoltStandard { get; set; }

    public List<PmcStandardInfo>? NutStandard { get; set; }

    public List<PmcStandardInfo>? WasherStandard { get; set; }

    public string? Status { get; set; }

    public int VersionNum { get; set; }

    public bool IsByRule { get; set; }

    public string? JsonData { get; set; }
}
