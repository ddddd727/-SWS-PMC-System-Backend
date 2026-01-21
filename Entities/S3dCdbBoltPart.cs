using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbBoltPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public int? MaterialGrade { get; set; }

    public int? GeometricIndustryStandard { get; set; }

    public int? BoltType { get; set; }
}
