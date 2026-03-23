using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Models
{
    /// <summary>
    /// 简化的标准配置（仅包含标准名称和材料信息，不包含通径范围）
    /// </summary>
    public class SimpleStandardConfig
    {
        /// <summary>
        /// 标准文件ID或名称（必填）
        /// </summary>
        [Required(ErrorMessage = "标准文件不能为空")]
        public object StandardFile { get; set; } = null!;

        /// <summary>
        /// 材料ID或名称（必填）
        /// </summary>
        [Required(ErrorMessage = "材料不能为空")]
        public object Material { get; set; } = null!;
    }
}
