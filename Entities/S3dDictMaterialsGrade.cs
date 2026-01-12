using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictMaterialsGrade
{
    public int Id { get; set; }

    public int MaterialsGradeCl { get; set; }

    public string MaterialsGradeCode { get; set; } = null!;

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
