namespace PMCSystem_Backend.Shared.Entities;

/// <summary>
/// 跨模块共享实体，映射表 S3D_Dict_ComponentType
/// 被 DesignRules、PipingSpecifications 等模块共用
/// </summary>
public partial class S3dDictPipingComponentType
{
    public int Id { get; set; }

    public string ComponentTypeName { get; set; } = null!;

    public string ComponentTypeDescription { get; set; } = null!;

    public bool Status { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
