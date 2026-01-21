using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePreferredCapScrewLength
{
    public int Id { get; set; }

    public decimal BoltDiameterFrom { get; set; }

    public decimal BoltDiameterTo { get; set; }

    public decimal BoltDiameterIncrement { get; set; }

    public string MaterialsGrade { get; set; } = null!;

    public decimal PreferredBoltLengthFrom { get; set; }

    public decimal PreferredBoltLengthTo { get; set; }

    public decimal PreferredBoltLengthIncrement { get; set; }
}
