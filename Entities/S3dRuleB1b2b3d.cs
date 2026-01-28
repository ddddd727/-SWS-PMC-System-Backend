using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleB1b2b3d
{
    public int Id { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int MaterialsGradeCl { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public string RuleName { get; set; } = null!;

    public bool Status { get; set; }
}