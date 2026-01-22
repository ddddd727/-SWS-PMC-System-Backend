using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPipeCompGeometryDatum
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

    public double? DryWeight { get; set; }

    public double? DryCogX { get; set; }

    public double? DryCogY { get; set; }

    public double? DryCogZ { get; set; }

    public string? PartDescription { get; set; }

    public string? MaterialsMgmtIdent { get; set; }

    public string? BendRadius { get; set; }

    public string? JsonData { get; set; }
}
