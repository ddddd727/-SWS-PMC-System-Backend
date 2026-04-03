namespace PMCSystem_Backend.Shared.Entities;

/// <summary>
/// 跨模块共享实体，映射表 S3D_Common_CodeListTable
/// </summary>
public partial class S3dCommonCodeListTable
{
    public int Id { get; set; }

    public string CodeListTableName { get; set; } = null!;

    public bool IsUserDefined { get; set; }

    public string Major { get; set; } = null!;

    public virtual ICollection<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; } = new List<S3dCommonCodeListValue>();
}
