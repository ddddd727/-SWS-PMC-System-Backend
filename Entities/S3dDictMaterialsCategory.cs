using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictMaterialsCategory
{
    public int Id { get; set; }

    public int MaterialsCategoryCl { get; set; }

    public string MaterialsCategoryCode { get; set; } = null!;

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
