using System.ComponentModel.DataAnnotations;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Models;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Requests
{
    /// <summary>
    /// 管系规格书保存请求
    /// </summary>
    public class SavePipeSpecRequest
    {
        [Required(ErrorMessage = "船型不能为空")]
        [MaxLength(255, ErrorMessage = "船型长度不能超过255个字符")]
        public string ShipType { get; set; } = string.Empty;

        [Required(ErrorMessage = "船号不能为空")]
        [MaxLength(255, ErrorMessage = "船号长度不能超过255个字符")]
        public string ShipNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "PMC编码不能为空")]
        [MaxLength(255, ErrorMessage = "PMC编码长度不能超过255个字符")]
        public string PmcCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "请至少配置一个部件类型")]
        [MinLength(1, ErrorMessage = "请至少配置一个部件类型")]
        public List<ComponentTypeConfiguration> Configurations { get; set; } = new List<ComponentTypeConfiguration>();

        /// <summary>
        /// 可选元数据
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
