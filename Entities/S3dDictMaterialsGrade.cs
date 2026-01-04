using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictMaterialsGrade
{
    public int Id { get; set; }

    public int MaterialsGradeCl { get; set; }

    public string? MaterialsGradeCode { get; set; }

    public bool Status { get; set; }
}
