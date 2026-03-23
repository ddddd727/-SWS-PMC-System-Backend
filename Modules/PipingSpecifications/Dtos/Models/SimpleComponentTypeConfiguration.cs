using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Models
{
    /// <summary>
    /// 简化的部件类型配置（仅包含标准名称和材料信息，不包含通径范围）
    /// </summary>
    public class SimpleComponentTypeConfiguration
    {
        /// <summary>
        /// 部件类型 ID（S3D_Dict_PipingComponentType.ID），推荐使用；与 ComponentType 二选一
        /// </summary>
        public int? ComponentTypeId { get; set; }

        /// <summary>
        /// 部件类型名称（展示或兼容旧请求，保存时优先以 ComponentTypeId 为准）
        /// </summary>
        public string? ComponentType { get; set; }

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
