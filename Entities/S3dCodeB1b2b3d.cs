using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCodeB1b2b3d
{
    public int Id { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public int MaterialsGradeCl { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public string RuleName { get; set; } = null!;

    public bool Status { get; set; }

    public string? MaterialsCategoryCode { get; set; }

    public string? PipingStandardCode { get; set; }

    public string? MaterialsGradeCode { get; set; }

    public string? ScheduleThicknessCode { get; set; }
}
