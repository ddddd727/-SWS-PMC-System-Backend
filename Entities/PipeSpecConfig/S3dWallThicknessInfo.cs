using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities.TempEntities;

public partial class S3dWallThicknessInfo
{
    public int Id { get; set; }

    public decimal NormalDiameter { get; set; }

    public string UnitType { get; set; } = null!;

    public int GeometricIndustryStandardCl { get; set; }

    public string GeometricIndustryStandard { get; set; } = null!;

    public int ScheduleThicknessCl { get; set; }

    public string ScheduleThickness { get; set; } = null!;

    public decimal PipingOutsideDiameter { get; set; }

    public decimal WallThickness { get; set; }

    public bool Status { get; set; }

    public string? Version { get; set; }
}
