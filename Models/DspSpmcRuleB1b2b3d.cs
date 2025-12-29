using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcRuleB1b2b3d
{
    public int Id { get; set; }

    public string? MaterialsCategoryId { get; set; }

    public int PipingStandardId { get; set; }

    public int MaterialsGradeId { get; set; }

    public int ScheduleThicknessId { get; set; }

    public string? RuleName { get; set; }
}
