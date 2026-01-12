using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dClScheduleThickness
{
    public int CodeListNumber { get; set; }

    public string ShortStringValue { get; set; } = null!;

    public string LongStringValue { get; set; } = null!;
}
