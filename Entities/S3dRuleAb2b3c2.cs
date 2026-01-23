using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleAb2b3c2
{
    public int Id { get; set; }

    public int PipingClassCl { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int MaterialsGradeCl { get; set; }

    public int PressureRatingCl { get; set; }

    public string? RuleName { get; set; }

    public bool Status { get; set; }
}