using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleSimpleSelectionSeriesStandsMap
{
    public int Id { get; set; }

    public int SimpleSelectionSeriesId { get; set; }

    public int PipingCommodityTypeCl { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public string? TechnicalRequirement { get; set; }

    public string? Version { get; set; }

    public bool Status { get; set; }

    public virtual S3dDictSimpleSelectionSeries SimpleSelectionSeries { get; set; } = null!;
}
