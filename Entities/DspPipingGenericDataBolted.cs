using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspPipingGenericDataBolted
{
    public int Id { get; set; }

    public decimal NominalPipingDiameter { get; set; }

    public string NominalDiameterUnits { get; set; } = null!;

    public int PressureRatingCl { get; set; }

    public int EndPreparationCl { get; set; }

    public int EndStandardCl { get; set; }

    public decimal? FlangeOutsideDiameter { get; set; }

    public decimal? FlangeThickness { get; set; }

    public decimal? FlangeThicknessTolerance { get; set; }

    public decimal? FlangeFaceProjection { get; set; }

    public decimal? RaisedFaceDiameter { get; set; }

    public decimal? FlangeGrooveWidth { get; set; }

    public decimal? SeatingDepth { get; set; }

    public decimal? BoltCircleDiameter { get; set; }

    public int? QuantityOfBoltsRequired { get; set; }

    public decimal? BoltDiameter { get; set; }

    public decimal? BodyOutsideDiameter { get; set; }

    public string? DrillingTemplatePattern { get; set; }
}
