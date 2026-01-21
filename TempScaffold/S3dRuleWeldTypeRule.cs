using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleWeldTypeRule
{
    public int Id { get; set; }

    public string FabricationTypeOfEnd1 { get; set; } = null!;

    public string ConstructionRequirementOfEnd1 { get; set; } = null!;

    public string FabricationTypeOfEnd2 { get; set; } = null!;

    public string ConstructionRequirementOfEnd2 { get; set; } = null!;

    public string WeldType { get; set; } = null!;
}
