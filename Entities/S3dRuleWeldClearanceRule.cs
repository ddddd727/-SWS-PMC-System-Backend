using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleWeldClearanceRule
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal NominalPipingDiameterFrom { get; set; }

    public decimal NominalPipingDiameterTo { get; set; }

    public string NominalPipingDiameterUnits { get; set; } = null!;

    public string WeldClass { get; set; } = null!;

    public decimal WeldClearanceRadiusIncrease { get; set; }

    public decimal WeldClearanceLength { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
