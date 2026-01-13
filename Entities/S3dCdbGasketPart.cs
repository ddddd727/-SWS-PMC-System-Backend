using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbGasketPart
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public decimal? NominalDiameterFrom { get; set; }

    public decimal? NominalDiameterTo { get; set; }

    public decimal? NominalDiameter { get; set; }

    public string? NpdUnitType { get; set; }

    public string GeometricIndustryStandard { get; set; } = null!;

    public string? MaterialGrade { get; set; }

    public string? GasketType { get; set; }

    public string? ThicknessFor3Dmodel { get; set; }

    public string? ProcurementThickness { get; set; }

    public string? GasketOutsideDiameter { get; set; }

    public string? GasketInsideDiameter { get; set; }

    public string? FlangeFacing { get; set; }
}
