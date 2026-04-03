using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Models;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos;

/// <summary>
/// 管系规格书版本详情 DTO
/// </summary>
public class PipeSpecVersionDetailDto
{
    /// <summary>版本记录主键</summary>
    public int Id { get; set; }

    /// <summary>版本号</summary>
    public int Version { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>创建人</summary>
    public string? CreatedBy { get; set; }

    /// <summary>版本备注</summary>
    public string? Comment { get; set; }

    /// <summary>PMC 基础信息</summary>
    public PmcBaseInfoDto BaseInfo { get; set; } = null!;

    /// <summary>部件类型配置列表</summary>
    public List<ComponentTypeConfiguration> Configurations { get; set; } = new List<ComponentTypeConfiguration>();

    /// <summary>规格书配置状态</summary>
    public string ConfigStatus { get; set; } = "pending";

    /// <summary>是否已配置</summary>
    public bool IsConfigured => Configurations != null && Configurations.Any();
}
