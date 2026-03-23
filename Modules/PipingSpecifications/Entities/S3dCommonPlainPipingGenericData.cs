using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

public partial class S3dCommonPlainPipingGenericData
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
