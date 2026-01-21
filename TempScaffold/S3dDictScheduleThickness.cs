using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dDictScheduleThickness
{
    public int Id { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public string ScheduleThicknessCode { get; set; } = null!;

    public int MaterialsCategoryCl { get; set; }

    public int ScheduleSeriesId { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
