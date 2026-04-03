using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

public partial class PmcSpecRuleData
{
    // 数据库ID
    public int Id { get; set; }

    // PMC编码
    public string? PmcCode { get; set; }        

    // 船号
    public string? ShipNo { get; set; }

    // PMC 状态码
    public string? Status { get; set; } 

    // PMC 主材料
    public string? Material { get; set; }

    // PMC 管材标准
    public string? PipeStandard { get; set; }

    public string? ElbowStandard { get; set; }

    public string? RedStandard { get; set; }

    public string? TeeStandard { get; set; }

    public string? SleeveStandard { get; set; }

    public string? BossesStandard { get; set; }

    public string? SaddlesStandard { get; set; }

    public string? CapsStandard { get; set; }

    public string? OverpassStandard { get; set; }

    public string? AccessoriesStandard { get; set; }

    public string? FlangeStandard { get; set; }

    public string? BlindFlangeStandard { get; set; }

    public string? GasketStandard { get; set; }

    public string? BoltStandard { get; set; }

    public string? NutStandard { get; set; }

    public string? WasherStandard { get; set; }
}