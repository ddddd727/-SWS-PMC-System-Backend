using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests
{
    /// <summary>
    /// 获取NPD信息请求
    /// </summary>
    public class GetNPDInfoRequest
    {
        /// <summary>
        /// 端面标准
        /// </summary>
        [Required(ErrorMessage = "端面标准不能为空")]
        [MaxLength(255, ErrorMessage = "端面标准长度不能超过255个字符")]
        public string EndStandard { get; set; } = string.Empty;

        /// <summary>
        /// 壁厚系列
        /// </summary>
        [Required(ErrorMessage = "壁厚系列不能为空")]
        [MaxLength(255, ErrorMessage = "壁厚系列长度不能超过255个字符")]
        public string Schedule { get; set; } = string.Empty;
    }
}
