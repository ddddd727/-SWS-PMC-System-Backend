using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleBoltExtension
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal NominalPipingDiameter { get; set; }

    public string NominalPipingDiameterUnits { get; set; } = null!;

    public string PressureRating { get; set; } = null!;

    public string? EndPreparation { get; set; }

    public string? EndStandard { get; set; }

    public decimal? StandardBoltExtensionForStuds { get; set; }

    public decimal? AltBoltExtensionForStuds2 { get; set; }

    public decimal? AltBoltExtensionForStuds3 { get; set; }

    public decimal? AltBoltExtensionForStuds4 { get; set; }

    public decimal? AltBoltExtensionForStuds5 { get; set; }

    public decimal? AltBoltExtensionForStuds6 { get; set; }

    public decimal? StandardBoltExtForMachBolts { get; set; }

    public decimal? AltBoltExtensionForMachBolts2 { get; set; }

    public decimal? AltBoltExtensionForMachBolts3 { get; set; }

    public decimal? AltBoltExtensionForMachBolts4 { get; set; }

    public decimal? AltBoltExtensionForMachBolts5 { get; set; }

    public decimal? AltBoltExtensionForMachBolts6 { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
