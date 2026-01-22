using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCodeWallThickness
{
    public int Id { get; set; }

    public decimal Npd { get; set; }

    public string Ndpunit { get; set; } = null!;

    public int ScheduleThicknessCl { get; set; }

    public string ScheduleThickness { get; set; } = null!;

    public int EndStandardCl { get; set; }

    public string EndStandard { get; set; } = null!;

    public decimal PipingOutsideDiameter { get; set; }

    public decimal WallThickness { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
