using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dCodePipingStandardPressureRating
{
    public string? PipingStandardCode { get; set; }

    public string? PipingStandardDesc { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public string? PressureRatingCode { get; set; }

    public string? PressureRatingDesc { get; set; }

    public int PressureRatingCl { get; set; }
}
