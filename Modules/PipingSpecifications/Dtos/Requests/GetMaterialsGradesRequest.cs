using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests
{
    /// <summary>
    /// 获取材料牌号请求（按主材料筛选）
    /// </summary>
    public class GetMaterialsGradesRequest
    {
        /// <summary>
        /// 主材料名称（Codelist: MaterialsCategory 的 ShortStringValue）
        /// </summary>
        [Required(ErrorMessage = "主材料名称不能为空")]
        [MaxLength(255, ErrorMessage = "主材料名称长度不能超过255个字符")]
        public string MaterialCategory { get; set; } = string.Empty;
    }
}
