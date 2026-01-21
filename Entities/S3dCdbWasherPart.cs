using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbWasherPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public int? WasherType { get; set; }

    public int? GeometricIndustryStandard { get; set; }

    public int? MaterialGrade { get; set; }

    public string? WasherThickness { get; set; }
}
