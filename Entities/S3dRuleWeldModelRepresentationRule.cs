using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleWeldModelRepresentationRule
{
    public int Id { get; set; }

    public decimal NominalPipingDiameterFrom { get; set; }

    public decimal NominalPipingDiameterTo { get; set; }

    public string NominalPipingDiameterUnits { get; set; } = null!;

    public string WeldClass { get; set; } = null!;

    public decimal WeldRadiusIncrease { get; set; }

    public decimal WeldThickness { get; set; }

    public string MaterialsGrade { get; set; } = null!;
}
