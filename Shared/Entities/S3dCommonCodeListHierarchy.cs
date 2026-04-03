namespace PMCSystem_Backend.Shared.Entities;

/// <summary>
/// 跨模块共享实体，映射表 S3D_Common_CodeListHierarchy
/// </summary>
public partial class S3dCommonCodeListHierarchy
{
    public int Id { get; set; }

    public int CodeListTableId { get; set; }

    public int? ParentCodeListTableId { get; set; }
}
