using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCodePipingStandardMaterialsGrade
{
    public string? PipingStandardCode { get; set; }

    public string PipeStandDesc { get; set; } = null!;

    public int GeometricIndustryStandardCl { get; set; }

    public string MaterialsGradeCode { get; set; } = null!;

    public string MaterialsGradeDesc { get; set; } = null!;

    public int MaterialsGradeCl { get; set; }
}
