using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleReinforcingWeldDatum
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal HeaderSize { get; set; }

    public decimal BranchSize { get; set; }

    public string? HeaderSizeUnitsOfMeasure { get; set; }

    public string? BranchSizeUnitsOfMeasure { get; set; }

    public decimal? AcuteBranchAngleFrom { get; set; }

    public decimal? AcuteBranchAngleTo { get; set; }

    public decimal? MinimumReinforcingWeldSize { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
