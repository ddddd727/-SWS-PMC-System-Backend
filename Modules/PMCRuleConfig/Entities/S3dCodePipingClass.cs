using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

public partial class S3dCodePipingClass
{
    public int Id { get; set; }

    public string PipingClassCode { get; set; } = null!;

    public string ShortStringValue { get; set; } = null!;

    public int CodeListNumber { get; set; }
}

