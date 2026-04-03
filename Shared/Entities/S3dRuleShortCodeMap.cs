namespace PMCSystem_Backend.Shared.Entities;

/// <summary>
/// 跨模块共享实体，映射表 S3D_Rule_ShortCodeMap
/// </summary>
public partial class S3dRuleShortCodeMap
{
    public int Id { get; set; }

    public int ComponentTypeId { get; set; }

    public string ShortCode { get; set; } = null!;
}
