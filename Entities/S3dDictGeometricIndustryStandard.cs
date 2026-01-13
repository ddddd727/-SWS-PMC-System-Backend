using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictGeometricIndustryStandard
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int ComponentTypeId { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }

    public virtual S3dDictPipingComponentType ComponentType { get; set; } = null!;
}
