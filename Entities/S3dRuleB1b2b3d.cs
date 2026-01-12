using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleB1b2b3d
{
    public int Id { get; set; }

    public int MaterialsCategoryId { get; set; }

    public int PipingStandardId { get; set; }

    public int MaterialsGradeId { get; set; }

    public int ScheduleThicknessId { get; set; }

    public string RuleName { get; set; } = null!;

    public bool Status { get; set; }
}
