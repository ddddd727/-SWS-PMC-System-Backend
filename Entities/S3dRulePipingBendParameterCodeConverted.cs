using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePipingBendParameterCodeConverted
{
    public int Id { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public string MaterialsCategory { get; set; } = null!;

    public decimal NormalDiameter { get; set; }

    public string UnitType { get; set; } = null!;

    public int ScheduleThicknessCl { get; set; }

    public string ScheduleThickness { get; set; } = null!;

    public decimal BendRadiusMultiplier { get; set; }

    public bool Status { get; set; }
}

