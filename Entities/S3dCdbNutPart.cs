using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbNutPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public string? MaterialGrade { get; set; }

    public string? GeometricIndustryStandard { get; set; }

    public string? NutType { get; set; }

    public string? NutHeight { get; set; }
}
