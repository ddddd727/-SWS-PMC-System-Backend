using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPipeGeometryDatum
{
    public int Id { get; set; }

    public string? IndustryCommodityCode { get; set; }

    public decimal? Npd1 { get; set; }

    public string? NpdUnitType1 { get; set; }

    public string? EndPreparation1 { get; set; }

    public string? ScheduleThickness1 { get; set; }

    public decimal? Npd2 { get; set; }

    public string? NpdUnitType2 { get; set; }

    public string? EndPreparation2 { get; set; }

    public string? ScheduleThickness2 { get; set; }

    public string? GeometricIndustryStandard { get; set; }

    public decimal? Density { get; set; }

    public decimal? PurchaseLength { get; set; }

    public decimal? MinimumPipeLength { get; set; }

    public decimal? MaximumPipeLength { get; set; }

    public decimal? WeightPerUnitLength { get; set; }

    public string? PartDescription { get; set; }

    public string? MaterialsMgmtIdent { get; set; }

    public string? JsonData { get; set; }
}
