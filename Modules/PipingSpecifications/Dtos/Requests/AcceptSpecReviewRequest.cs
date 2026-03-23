using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests
{
    /// <summary>
    /// 接受规格书审核请求（占位，默认审核成功，后续接入审核系统）
    /// </summary>
    public class AcceptSpecReviewRequest
    {
        /// <summary>
        /// PMC编码（必填）
        /// </summary>
        [Required(ErrorMessage = "PMC编码不能为空")]
        [MaxLength(255, ErrorMessage = "PMC编码长度不能超过255个字符")]
        public string PmcCode { get; set; } = string.Empty;

        /// <summary>
        /// 船型（可选，与 ShipNumber 同时提供时精确匹配记录）
        /// </summary>
        [MaxLength(255)]
        public string? ShipType { get; set; }

        /// <summary>
        /// 船号（可选）
        /// </summary>
        [MaxLength(255)]
        public string? ShipNumber { get; set; }
    }
}
