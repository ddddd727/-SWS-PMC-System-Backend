using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcDictPipingClass
{
    public int Id { get; set; }

    public int PipingClassCl { get; set; }

    public string? PipingClassCode { get; set; }

    public bool Status { get; set; }
}
