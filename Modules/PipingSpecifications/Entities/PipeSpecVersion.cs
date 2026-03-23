using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

/// <summary>
/// 管系规格书历史版本快照
/// </summary>
public partial class PipeSpecVersion
{
    /// <summary>主键</summary>
    public int Id { get; set; }

    /// <summary>PMC 编码</summary>
    public string PmcCode { get; set; } = null!;

    /// <summary>船型</summary>
    public string ShipType { get; set; } = null!;

    /// <summary>船号</summary>
    public string ShipNo { get; set; } = null!;

    /// <summary>版本号（同一 PMC+船型+船号 下递增）</summary>
    public int Version { get; set; }

    /// <summary>管系等级名称</summary>
    public string? PipingClassName { get; set; }

    /// <summary>材料类别名称</summary>
    public string? MaterialsCategoryName { get; set; }

    /// <summary>管材标准名称</summary>
    public string? PipingStandardName { get; set; }

    /// <summary>材料等级名称</summary>
    public string? MaterialsGradeName { get; set; }

    /// <summary>法兰标准名称</summary>
    public string? FlangeStandardName { get; set; }

    /// <summary>压力等级名称</summary>
    public string? PressureRatingName { get; set; }

    /// <summary>壁厚系列名称</summary>
    public string? ScheduleThicknessName { get; set; }

    /// <summary>管材标准配置</summary>
    public List<PmcStandardInfo>? PipeStandard { get; set; }

    /// <summary>弯头标准配置</summary>
    public List<PmcStandardInfo>? ElbowStandard { get; set; }

    /// <summary>异径管标准配置</summary>
    public List<PmcStandardInfo>? RedStandard { get; set; }

    /// <summary>三通标准配置</summary>
    public List<PmcStandardInfo>? TeeStandard { get; set; }

    /// <summary>套管标准配置</summary>
    public List<PmcStandardInfo>? SleeveStandard { get; set; }

    /// <summary>支管台标准配置</summary>
    public List<PmcStandardInfo>? BossesStandard { get; set; }

    /// <summary>鞍座标准配置</summary>
    public List<PmcStandardInfo>? SaddlesStandard { get; set; }

    /// <summary>管帽标准配置</summary>
    public List<PmcStandardInfo>? CapsStandard { get; set; }

    /// <summary>跨越件标准配置</summary>
    public List<PmcStandardInfo>? OverpassStandard { get; set; }

    /// <summary>附件标准配置</summary>
    public List<PmcStandardInfo>? AccessoriesStandard { get; set; }

    /// <summary>法兰标准配置</summary>
    public List<PmcStandardInfo>? FlangeStandard { get; set; }

    /// <summary>盲板标准配置</summary>
    public List<PmcStandardInfo>? BlindFlangeStandard { get; set; }

    /// <summary>垫片标准配置</summary>
    public List<PmcStandardInfo>? GasketStandard { get; set; }

    /// <summary>螺栓标准配置</summary>
    public List<PmcStandardInfo>? BoltStandard { get; set; }

    /// <summary>螺母标准配置</summary>
    public List<PmcStandardInfo>? NutStandard { get; set; }

    /// <summary>垫圈标准配置</summary>
    public List<PmcStandardInfo>? WasherStandard { get; set; }

    /// <summary>配置状态</summary>
    public string? Status { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>创建人</summary>
    public string? CreatedBy { get; set; }

    /// <summary>版本备注</summary>
    public string? Comment { get; set; }
}
