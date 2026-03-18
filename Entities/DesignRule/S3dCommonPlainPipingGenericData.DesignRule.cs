using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities.DesignRule;

public partial class S3dCommonPlainPipingGenericData
{
    public int Id { get; set; }

    public double NominalPipingDiameter { get; set; }

    public string NominalDiameterUnits { get; set; } = null!;

    public int EndStandardCl { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public int PressureRatingCl { get; set; }

    public string? PipingOutsideDiameter { get; set; }

    public string? WallThickness { get; set; }

    public bool Status { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
