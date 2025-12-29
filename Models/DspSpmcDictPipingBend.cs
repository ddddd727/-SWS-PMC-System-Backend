using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcDictPipingBend
{
    public int Id { get; set; }

    public decimal? OutSideDiameter { get; set; }

    public string? OutSideDiameterUnit { get; set; }

    public decimal? HeaderClampLength { get; set; }

    public decimal? TailClampLength { get; set; }
}
