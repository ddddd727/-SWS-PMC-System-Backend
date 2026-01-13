using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleMinPipeLengthPurchasePerSpec
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal NominalPipingDiameter { get; set; }

    public string NominalPipingDiameterUnits { get; set; } = null!;

    public decimal PurchaseLength { get; set; }

    public decimal MinimumPipeLength { get; set; }

    public decimal? PreferredMinimumPipeLength { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
