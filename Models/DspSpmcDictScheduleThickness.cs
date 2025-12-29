using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcDictScheduleThickness
{
    public int Id { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public string? ScheduleThicknessCode { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public bool Status { get; set; }
}
