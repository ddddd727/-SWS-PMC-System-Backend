using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCommonPipingGenericDataFemale
{
    public int Id { get; set; }

    public decimal NominalPipingDiameter { get; set; }

    public string NominalDiameterUnits { get; set; } = null!;

    public int PressureRatingCl { get; set; }

    public int EndPreparationCl { get; set; }

    public int EndStandardCl { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public decimal? SocketDiameter { get; set; }

    public decimal? SocketDepth { get; set; }

    public decimal? SocketOffset { get; set; }

    public decimal? ThreadDepth { get; set; }

    public decimal? HubOutsideDiameter { get; set; }

    public decimal? HubThickness { get; set; }

    public decimal? BodyOutsideDiameter { get; set; }
}
