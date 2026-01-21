using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dDictScheduleSeries
{
    public int Id { get; set; }

    public string ScheduleSeriesName { get; set; } = null!;

    public bool Status { get; set; }
}
