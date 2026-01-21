using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPipeGeometryDatum
{
    public int Id { get; set; }

    public string? IndustryCommodityCode { get; set; }

    public double? Npd1 { get; set; }

    public string? NpdUnitType1 { get; set; }

    public int? EndPreparation1 { get; set; }

    public int? ScheduleThickness1 { get; set; }

    public double? Npd2 { get; set; }

    public string? NpdUnitType2 { get; set; }

    public int? EndPreparation2 { get; set; }

    public int? ScheduleThickness2 { get; set; }

    public int? GeometricIndustryStandard { get; set; }

    public double? Density { get; set; }

    public double? PurchaseLength { get; set; }

    public double? MinimumPipeLength { get; set; }

    public double? MaximumPipeLength { get; set; }

    public double? WeightPerUnitLength { get; set; }

    public string? PartDescription { get; set; }

    public string? MaterialsMgmtIdent { get; set; }

    public string? JsonData { get; set; }
}
