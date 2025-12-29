using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcRulePressureRating
{
    public int Id { get; set; }

    public int PressureRatingCl { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public bool Status { get; set; }
}
