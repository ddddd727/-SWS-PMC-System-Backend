using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePipingCommodityFilter
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public string ShortCode { get; set; } = null!;

    public string OptionCode { get; set; } = null!;

    public decimal FirstSizeFrom { get; set; }

    public decimal FirstSizeTo { get; set; }

    public string FirstSizeUnits { get; set; } = null!;

    public string? FirstSizeSchedule { get; set; }

    public decimal? SecondSizeFrom { get; set; }

    public decimal? SecondSizeTo { get; set; }

    public string? SecondSizeUnits { get; set; }

    public string? SecondSizeSchedule { get; set; }

    public string? MultisizeOption { get; set; }

    public string SelectionBasis { get; set; } = null!;

    public string? EngineeringTag { get; set; }

    public string CommodityCode { get; set; } = null!;

    public decimal? BendRadiusMultiplier { get; set; }

    public decimal? BendRadius { get; set; }

    public int? NumberOfMiterCuts { get; set; }

    public string? PipingNote1 { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
