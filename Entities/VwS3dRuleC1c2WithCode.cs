using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class VwS3dRuleC1c2WithCode
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int PressureRatingCl { get; set; }

    public string RuleName { get; set; } = null!;

    public bool Status { get; set; }

    public string? FlangeStandardCode { get; set; }

    public string? PressureRatingCode { get; set; }
}
