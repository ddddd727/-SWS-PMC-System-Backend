using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePressureRating
{
    public int Id { get; set; }

    public int PressureRatingCl { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
