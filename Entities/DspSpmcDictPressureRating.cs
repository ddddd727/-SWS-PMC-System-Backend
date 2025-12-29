using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspSpmcDictPressureRating
{
    public int Id { get; set; }

    public int PressureRatingCl { get; set; }

    public string? PressureRatingCode { get; set; }

    public bool Status { get; set; }
}
