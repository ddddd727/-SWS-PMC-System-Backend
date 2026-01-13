using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCdbPortDatum
{
    public int Id { get; set; }

    public decimal? Npd1 { get; set; }

    public string? NpdUnitType1 { get; set; }

    public string? PressureRating1 { get; set; }

    public string? EndPreparation1 { get; set; }

    public string? EndStandard1 { get; set; }

    public string? SchduleThickness1 { get; set; }

    public string? FlowDirection1 { get; set; }

    public decimal? Npd2 { get; set; }

    public string? NpdUnitType2 { get; set; }

    public string? PressureRating2 { get; set; }

    public string? EndPreparation2 { get; set; }

    public string? EndStandard2 { get; set; }

    public string? SchduleThickness2 { get; set; }

    public string? FlowDirection2 { get; set; }
}
