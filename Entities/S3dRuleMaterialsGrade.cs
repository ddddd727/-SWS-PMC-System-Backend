using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleMaterialsGrade
{
    public int Id { get; set; }

    public int MaterialsGradeCl { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public int? GeometricIndustryStandardCl { get; set; }

    public string? Manufacturer { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
