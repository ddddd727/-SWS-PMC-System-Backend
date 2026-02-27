using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictPipingBendData
{
    public int Id { get; set; }

    public double OutSideDiameter { get; set; }

    public string OutSideDiameterUnit { get; set; } = null!;

    public double HeaderClampLength { get; set; }

    public double TailClampLength { get; set; }

    public int? MaterialsCategoryCl { get; set; }

    public double? BendRadius { get; set; }

    public double? MaxPipeLength { get; set; }

    public string? WallThicknessFrom { get; set; }

    public string? WallThicknessTo { get; set; }

    public int? MachineNum { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
