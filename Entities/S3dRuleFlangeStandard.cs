using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleFlangeStandard
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public string FlangeStandardCode { get; set; } = null!;

    public int? GeometricIndustryPracticeCl { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
