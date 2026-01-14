using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPipeStock
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public string? CommodityType { get; set; }

    public string? MaterialGrade { get; set; }

    public string? GeometricIndustryStandard { get; set; }
}
