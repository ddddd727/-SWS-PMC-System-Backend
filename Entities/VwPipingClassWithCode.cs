using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class VwPipingClassWithCode
{
    public int Id { get; set; }

    public string PipingClassCode { get; set; } = null!;

    public string ShortStringValue { get; set; } = null!;
}
