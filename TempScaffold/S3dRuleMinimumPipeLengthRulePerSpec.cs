using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleMinimumPipeLengthRulePerSpec
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal Npd { get; set; }

    public string NpdUnitType { get; set; } = null!;

    public decimal MinimumPipeLength { get; set; }

    public decimal? PreferredMinimumPipeLength { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
