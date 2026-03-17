namespace PMCSystem_Backend.Dtos.PipeSpecConfig;

/// <summary>
/// 管系规格书版本列表项 DTO
/// </summary>
public class PipeSpecVersionDto
{
    /// <summary>版本记录主键</summary>
    public int Id { get; set; }

    /// <summary>PMC 编码</summary>
    public string PmcCode { get; set; } = null!;

    /// <summary>船型</summary>
    public string ShipType { get; set; } = null!;

    /// <summary>船号</summary>
    public string ShipNo { get; set; } = null!;

    /// <summary>版本号</summary>
    public int Version { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>创建人</summary>
    public string? CreatedBy { get; set; }

    /// <summary>版本备注</summary>
    public string? Comment { get; set; }
}
