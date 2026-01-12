using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleMatingPort
{
    public int Id { get; set; }

    public string EndPrep1 { get; set; } = null!;

    public string EndPrep2 { get; set; } = null!;
}
