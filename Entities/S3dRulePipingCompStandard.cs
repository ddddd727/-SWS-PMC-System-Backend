using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePipingCompStandard
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public string? ComponentType { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public bool Status { get; set; }
}
