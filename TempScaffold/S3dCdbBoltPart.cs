using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dCdbBoltPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public string? MaterialGrade { get; set; }

    public string? GeometricIndustryStandard { get; set; }

    public string? BoltType { get; set; }
}
