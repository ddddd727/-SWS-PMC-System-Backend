using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPortDatum
{
    public int Id { get; set; }

    public double? Npd1 { get; set; }

    public string? NpdUnitType1 { get; set; }

    public int? PressureRating1 { get; set; }

    public int? EndPreparation1 { get; set; }

    public int? EndStandard1 { get; set; }

    public int? SchduleThickness1 { get; set; }

    public int? FlowDirection1 { get; set; }

    public double? Npd2 { get; set; }

    public string? NpdUnitType2 { get; set; }

    public int? PressureRating2 { get; set; }

    public int? EndPreparation2 { get; set; }

    public int? EndStandard2 { get; set; }

    public int? SchduleThickness2 { get; set; }

    public int? FlowDirection2 { get; set; }
}
