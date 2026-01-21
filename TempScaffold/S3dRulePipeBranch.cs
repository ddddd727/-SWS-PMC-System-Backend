using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePipeBranch
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal HeaderSize { get; set; }

    public decimal BranchSize { get; set; }

    public decimal? AngleLow { get; set; }

    public decimal? AngleHigh { get; set; }

    public string? HdrSizeNpdunitType { get; set; }

    public string? BrSizeNpdunitType { get; set; }

    public string? ShortCode { get; set; }

    public string? SecondaryShortCode { get; set; }

    public string? TertiaryShortCode { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
