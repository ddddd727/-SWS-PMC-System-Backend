using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleDefaultChangeOfDirectionPerSpec
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal BendAngleFrom { get; set; }

    public decimal BendAngleTo { get; set; }

    public string FunctionalShortCode { get; set; } = null!;

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
