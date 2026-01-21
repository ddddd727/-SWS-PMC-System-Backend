using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleGasketSelectionFilter
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal NominalDiameterFrom { get; set; }

    public decimal NominalDiameterTo { get; set; }

    public string NpdUnitType { get; set; } = null!;

    public string? GasketOption { get; set; }

    public decimal? MaximumTemperature { get; set; }

    public decimal? MinimumTemperature { get; set; }

    public string? EndPreparation { get; set; }

    public string? PressureRating { get; set; }

    public string? EndStandard { get; set; }

    public string? AlternateEndPreparation { get; set; }

    public string? AlternatePressureRating { get; set; }

    public string? AlternateEndStandard { get; set; }

    public string? FluidCode { get; set; }

    public string? ScheduleThickness { get; set; }

    public string? ContractorCommodityCode { get; set; }

    public int? Priority { get; set; }

    public string? RingNumber { get; set; }

    public string? FabricationCategoryOverride { get; set; }

    public string? SupplyResponsibilityOverride { get; set; }

    public string? Comments { get; set; }

    public int? QuantityOfAltReportableParts { get; set; }

    public string? AltReportableCommodityCode { get; set; }

    public int? QuantityOfReportableParts { get; set; }

    public string? ReportableCommodityCode { get; set; }

    public string? PipingNote1 { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
