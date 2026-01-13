using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbWasherPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public string? MaterialGrade { get; set; }

    public string? GeometricIndustryStandard { get; set; }

    public string? WasherType { get; set; }

    public string? WasherThickness { get; set; }
}
