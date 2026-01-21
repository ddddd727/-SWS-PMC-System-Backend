using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleSlipOnFlangeSetbackDistance
{
    public int Id { get; set; }

    public decimal NominalPipingDiameterFrom { get; set; }

    public decimal NominalPipingDiameterTo { get; set; }

    public string NominalPipingDiameterUnits { get; set; } = null!;

    public string EndStandard { get; set; } = null!;

    public decimal CompanyPracticeGap { get; set; }

    public decimal CompanyPracticeRoundOffFactor { get; set; }

    public decimal MaximumWeldThickness { get; set; }
}
