using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePipingStandardSchMap
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public decimal NormalDiameter { get; set; }

    public string UnitType { get; set; } = null!;

    public decimal PipingOutsideDiameter { get; set; }

    public decimal WallThickness { get; set; }

    public string? Version { get; set; }

    public bool Status { get; set; }
}
