using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

public partial class S3dCodeFlangeStandPressureRating
{
    public string FlangeStandardCode { get; set; } = null!;

    public string FlangeStandDesc { get; set; } = null!;

    public int GeometricIndustryStandardCl { get; set; }

    public string? PressureRatingCode { get; set; }

    public string? PressureRatingDesc { get; set; }

    public int? PressureRatingCl { get; set; }
}
