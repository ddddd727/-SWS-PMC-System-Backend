using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class VwPipingStandardScheduleThickness
{
    public string? PipingStandardCode { get; set; }

    public string PipeStandDesc { get; set; } = null!;

    public int GeometricIndustryStandardCl { get; set; }

    public string ScheduleThicknessCode { get; set; } = null!;

    public string ScheduleThicknessDesc { get; set; } = null!;

    public int ScheduleThicknessCl { get; set; }
}
