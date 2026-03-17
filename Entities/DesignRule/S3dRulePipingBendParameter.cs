using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePipingBendParameter
{
    public int Id { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public int? GeometricIndustryStandardCl { get; set; }

    public int? MaterialsGradeCl { get; set; }

    public double NormalDiameter { get; set; }

    public string UnitType { get; set; } = null!;

    public string? WallThicknessFrom { get; set; }

    public string? WallThicknessTo { get; set; }

    public decimal BendRadiusMultiplier { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
