using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleAb2b3c2
{
    public int Id { get; set; }

    public int PipingClassId { get; set; }

    public int PipingStandardId { get; set; }

    public int MaterialsGradeId { get; set; }

    public int PressureRatingId { get; set; }

    public string? RuleName { get; set; }

    public bool Status { get; set; }
}
