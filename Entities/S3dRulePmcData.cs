using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePmcData
{
    public int Id { get; set; }

    public string? Pmccode { get; set; }

    public string? ShipType { get; set; }

    public string? Material { get; set; }

    public string? ShipNo { get; set; }

    public string? Status { get; set; }

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
}
