using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePipeNominalDiameter
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public decimal Npd { get; set; }

    public string? NpdUnitType { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
