using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRuleStudBoltLenCalTolerance
{
    public int Id { get; set; }

    public decimal BoltLengthFrom { get; set; }

    public decimal BoltLengthTo { get; set; }

    public decimal BoltDiameterFrom { get; set; }

    public decimal BoltDiameterTo { get; set; }

    public decimal BoltLengthTolerance { get; set; }
}
