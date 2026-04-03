using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

public partial class S3dCdbPipeStock
{
    public int Id { get; set; }

    public string IndustryCommodityCode { get; set; } = null!;

    public string? CommodityType { get; set; }

    public string? MaterialGrade { get; set; }

    public decimal? Density { get; set; }

    public decimal? WeightPerUnitLength { get; set; }

    public decimal? PurchaseLength { get; set; }

    public decimal? MinimumPipeLength { get; set; }

    public decimal? MaximumPipeLength { get; set; }

    public string? GeometricIndustryStandard { get; set; }
}
