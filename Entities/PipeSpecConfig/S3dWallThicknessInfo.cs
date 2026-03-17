using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities.TempEntities;

public partial class S3dWallThicknessInfo
{
    public int Id { get; set; }

    /// <summary>通径（视图可能返回 float/real，故用 double 避免 Double 到 Decimal 的转换异常）</summary>
    public double NormalDiameter { get; set; }

    public string UnitType { get; set; } = null!;

    public int GeometricIndustryStandardCl { get; set; }

    public string GeometricIndustryStandard { get; set; } = null!;

    public int ScheduleThicknessCl { get; set; }

    public string ScheduleThickness { get; set; } = null!;

    /// <summary>外径（视图可能返回 float/real，故用 double 避免 Double 到 Decimal 的转换异常）</summary>
    public double PipingOutsideDiameter { get; set; }

    /// <summary>壁厚（视图可能返回 float/real，故用 double 避免 Double 到 Decimal 的转换异常）</summary>
    public double WallThickness { get; set; }

    public bool Status { get; set; }

    public string? Version { get; set; }
}
