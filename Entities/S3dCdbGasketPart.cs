using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbGasketPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public double? NominalDiameterFrom { get; set; }

    public double? NominalDiameterTo { get; set; }

    public double? NominalDiameter { get; set; }

    public string? NpdUnitType { get; set; }

    public int GeometricIndustryStandard { get; set; }

    public int? MaterialGrade { get; set; }

    public int? GasketType { get; set; }

    public string? ThicknessFor3Dmodel { get; set; }

    public string? ProcurementThickness { get; set; }

    public string? GasketOutsideDiameter { get; set; }

    public string? GasketInsideDiameter { get; set; }

    public string? FlangeFacing { get; set; }
}
