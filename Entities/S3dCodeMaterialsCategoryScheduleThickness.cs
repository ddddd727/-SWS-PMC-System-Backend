using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCodeMaterialsCategoryScheduleThickness
{
    public string MaterialsCategoryCode { get; set; } = null!;

    public string MaterialsCategoryDesc { get; set; } = null!;

    public int MaterialsCategoryCl { get; set; }

    public string ScheduleThicknessCode { get; set; } = null!;

    public string ScheduleThicknessDesc { get; set; } = null!;

    public int ScheduleThicknessCl { get; set; }
}
