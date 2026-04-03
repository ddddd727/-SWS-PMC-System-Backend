using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

public partial class S3dRuleC1c2
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int PressureRatingCl { get; set; }

    public string RuleName { get; set; } = null!;

    public bool Status { get; set; }
}
