using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictGeometricIndustryPractice
{
    public int Id { get; set; }

    public int GeometricIndustryPracticeCl { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
