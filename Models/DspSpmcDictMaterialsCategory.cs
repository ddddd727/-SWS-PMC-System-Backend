using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcDictMaterialsCategory
{
    public int Id { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public string? MaterialsCategoryCode { get; set; }

    public bool Status { get; set; }
}
