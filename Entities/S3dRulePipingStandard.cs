using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePipingStandard
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public string? PipingStandardCode { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public int? GeometricIndustryPracticeCl { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
