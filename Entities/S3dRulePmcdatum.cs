using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePmcdatum
{
    public int Id { get; set; }

    public string ShipType { get; set; } = null!;

    public string ShipNo { get; set; } = null!;

    public string Pmccode { get; set; } = null!;

    public string PipingClassName { get; set; } = null!;

    public string MaterialsCategoryName { get; set; } = null!;

    public string PipingStandardName { get; set; } = null!;

    public string MaterialsGradeName { get; set; } = null!;

    public string FlangeStandardName { get; set; } = null!;

    public string PressureRatingName { get; set; } = null!;

    public string ScheduleThicknessName { get; set; } = null!;

}