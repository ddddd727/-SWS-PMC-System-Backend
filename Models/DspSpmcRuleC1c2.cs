using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcRuleC1c2
{
    public int Id { get; set; }

    public int FlangeStandardId { get; set; }

    public int PressureRatingId { get; set; }

    public string? RuleName { get; set; }
}
