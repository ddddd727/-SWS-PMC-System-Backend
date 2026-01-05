using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictPipingClass
{
    public int Id { get; set; }

    public int PipingClassCl { get; set; }

    public string? PipingClassCode { get; set; }

    public bool Status { get; set; }

}
