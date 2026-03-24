namespace PMCSystem_Backend.Shared.Entities;

/// <summary>
/// 跨模块共享实体，映射表 S3D_Common_PlainPipingGenericData
/// 采用 DesignRules 版结构（含审计字段），与当前数据库一致
/// </summary>
public partial class S3dCommonPlainPipingGenericData
{
    public int Id { get; set; }

    public double NominalPipingDiameter { get; set; }

    public string NominalDiameterUnits { get; set; } = null!;

    public int EndStandardCl { get; set; }

    public int ScheduleThicknessCl { get; set; }

    public int PressureRatingCl { get; set; }

    public string? PipingOutsideDiameter { get; set; }

    public string? WallThickness { get; set; }

    public bool Status { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
