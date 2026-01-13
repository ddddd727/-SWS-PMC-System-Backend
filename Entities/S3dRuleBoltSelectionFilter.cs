using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleBoltSelectionFilter
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal NominalDiameterFrom { get; set; }

    public decimal NominalDiameterTo { get; set; }

    public string? NpdUnitType { get; set; }

    public string? BoltOption { get; set; }

    public decimal? MaximumTemperature { get; set; }

    public string? EndPreparation { get; set; }

    public string? PressureRating { get; set; }

    public string? EndStandard { get; set; }

    public string? AlternateEndPreparation { get; set; }

    public string? AlternatePressureRating { get; set; }

    public string? AlternateEndStandard { get; set; }

    public string? ContractorCommodityCode { get; set; }

    public int? Priority { get; set; }

    public string? BoltExtensionOption { get; set; }

    public string? FabricationCategoryOverride { get; set; }

    public string? SupplyResponsibilityOverride { get; set; }

    public string? Comments { get; set; }

    public string? PipingNote1 { get; set; }

    public string? LubricationRequirements { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
