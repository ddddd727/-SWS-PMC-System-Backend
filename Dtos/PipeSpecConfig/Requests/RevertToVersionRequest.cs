using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Requests;

/// <summary>
/// 使用历史版本覆盖当前版本请求
/// </summary>
public class RevertToVersionRequest
{
    /// <summary>
    /// 船型（可选，与 ShipNumber 同时提供时精确匹配主表记录）
    /// </summary>
    [MaxLength(255, ErrorMessage = "船型长度不能超过255个字符")]
    public string? ShipType { get; set; }

    /// <summary>
    /// 船号（可选）
    /// </summary>
    [MaxLength(255, ErrorMessage = "船号长度不能超过255个字符")]
    public string? ShipNumber { get; set; }
}
