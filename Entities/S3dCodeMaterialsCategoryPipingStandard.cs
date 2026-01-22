using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCodeMaterialsCategoryPipingStandard
{
    public string MaterialsCategoryCode { get; set; } = null!;

    public string MaterialsCategoryDesc { get; set; } = null!;

    public int MaterialsCategoryCl { get; set; }

    public string? PipingStandardCode { get; set; }

    public string PipeStandDesc { get; set; } = null!;

    public int GeometricIndustryStandardCl { get; set; }
}
