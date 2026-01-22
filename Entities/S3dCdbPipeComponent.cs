using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPipeComponent
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public int? CommodityType { get; set; }

    public int? GeometryType { get; set; }

    public int? MaterialGrade { get; set; }

    public int? GeometricIndustryStandard { get; set; }

    public string? BendAngle { get; set; }

    public int? PartDataBasis { get; set; }

    public string? PartClassName { get; set; }
}
