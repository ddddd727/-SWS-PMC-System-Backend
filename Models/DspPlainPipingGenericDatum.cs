using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspPlainPipingGenericDatum
{
    public int Id { get; set; }

    public decimal NominalPipingDiameter { get; set; }

    public string NominalDiameterUnits { get; set; } = null!;

    public int EndStandardCl { get; set; }

    public int ScheduleCl { get; set; }

    public int PressureRatingCl { get; set; }

    public decimal? PipingOutsideDiameter { get; set; }

    public decimal? WallThickness { get; set; }
}
