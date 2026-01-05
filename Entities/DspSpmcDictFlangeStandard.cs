using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspSpmcDictFlangeStandard
{
    public int Id { get; set; }

    public int GeometricIndustryStandardCl { get; set; }

    public string? FlangeStandardCode { get; set; }

    public int? GeometricIndustryPracticeCl { get; set; }

    public bool Status { get; set; }
}
