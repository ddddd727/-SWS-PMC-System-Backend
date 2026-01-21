using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dDictPipingBendDatum
{
    public int Id { get; set; }

    public decimal OutSideDiameter { get; set; }

    public string OutSideDiameterUnit { get; set; } = null!;

    public decimal HeaderClampLength { get; set; }

    public decimal TailClampLength { get; set; }

    public int? MachineNum { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
