using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Requests
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
        public string EndStandard { get; set; } = string.Empty;

        /// <summary>
        /// 壁厚系列
        /// </summary>
        [Required(ErrorMessage = "壁厚系列不能为空")]
        public string Schedule { get; set; } = string.Empty;
    }
}
