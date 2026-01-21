using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleWasherSelectionFilter
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public string? WasherOption { get; set; }

    public decimal? MaximumTemperature { get; set; }

    public decimal BoltDiameter { get; set; }

    public string? PressureRating { get; set; }

    public string? ContractorCommodityCode { get; set; }

    public string? FabricationCategoryOverride { get; set; }

    public string? SupplyResponsibilityOverride { get; set; }

    public string? SupplementaryWasherReqmt { get; set; }

    public string? SupplWasherCntrCommodityCode { get; set; }

    public string? Comments { get; set; }

    public string? PipingNote1 { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
