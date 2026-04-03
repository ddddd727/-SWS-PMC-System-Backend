using System.ComponentModel.DataAnnotations;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Models;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests
{
    /// <summary>
    /// 简化的管系规格书保存请求（仅包含标准名称和材料信息，不包含通径范围）
    /// </summary>
    public class SavePipeSpecSimpleRequest
    {
        /// <summary>
        /// 船型（必填）
        /// </summary>
        [Required(ErrorMessage = "船型不能为空")]
        [MaxLength(255, ErrorMessage = "船型长度不能超过255个字符")]
        public string ShipType { get; set; } = string.Empty;

        /// <summary>
        /// 船号（必填）
        /// </summary>
        [Required(ErrorMessage = "船号不能为空")]
        [MaxLength(255, ErrorMessage = "船号长度不能超过255个字符")]
        public string ShipNumber { get; set; } = string.Empty;

        /// <summary>
        /// PMC编码（必填）
        /// </summary>
        [Required(ErrorMessage = "PMC编码不能为空")]
        [MaxLength(255, ErrorMessage = "PMC编码长度不能超过255个字符")]
        public string PmcCode { get; set; } = string.Empty;

        /// <summary>
        /// 部件类型配置列表（至少一个）
        /// </summary>
        [Required(ErrorMessage = "请至少配置一个部件类型")]
        [MinLength(1, ErrorMessage = "请至少配置一个部件类型")]
        public List<SimpleComponentTypeConfiguration> Configurations { get; set; } = new List<SimpleComponentTypeConfiguration>();

        /// <summary>
        /// 可选元数据
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
