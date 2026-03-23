using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

public partial class S3dCdbPipeComponent
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public string? CommodityType { get; set; }

    public string? GeometryType { get; set; }

    public string? MaterialGrade { get; set; }

    public string? GeometricIndustryStandard { get; set; }

    public decimal? BendAngle { get; set; }

    public string? PartDataBasis { get; set; }

    public string? PartClassName { get; set; }
}
