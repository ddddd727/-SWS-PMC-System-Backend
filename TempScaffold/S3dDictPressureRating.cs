using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dDictPressureRating
{
    public int Id { get; set; }

    public int PressureRatingCl { get; set; }

    public string PressureRatingCode { get; set; } = null!;

    public bool Status { get; set; }
}
