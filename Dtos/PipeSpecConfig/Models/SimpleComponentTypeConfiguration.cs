using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 简化的部件类型配置（仅包含标准名称和材料信息，不包含通径范围）
    /// </summary>
    public class SimpleComponentTypeConfiguration
    {
        /// <summary>
        /// 部件类型（必填）
        /// </summary>
        [Required(ErrorMessage = "部件类型不能为空")]
        public string ComponentType { get; set; } = string.Empty;

        /// <summary>
        /// 配置结果描述（可选）
        /// </summary>
        public string? ConfigResult { get; set; }

        /// <summary>
        /// 标准配置列表（至少一个）
        /// </summary>
        [Required(ErrorMessage = "请至少配置一个标准")]
        [MinLength(1, ErrorMessage = "请至少配置一个标准")]
        public List<SimpleStandardConfig> Standards { get; set; } = new List<SimpleStandardConfig>();
    }
}
