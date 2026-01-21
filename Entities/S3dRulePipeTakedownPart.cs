using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRulePipeTakedownPart
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public string TakeDownShortCode { get; set; } = null!;

    public string WeldShortCode { get; set; } = null!;

    public bool IsPairRequired { get; set; }

    public decimal Npd { get; set; }

    public string NpdUnitType { get; set; } = null!;

    public bool IsWeld { get; set; }

    public virtual S3dRulePipingMaterialsClassDatum Spec { get; set; } = null!;
}
