using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 部件类型配置
    /// </summary>
    public class ComponentTypeConfiguration
    {
        [Required(ErrorMessage = "部件类型不能为空")]
        public string ComponentType { get; set; } = string.Empty;

        /// <summary>
        /// 配置结果描述
        /// </summary>
        public string? ConfigResult { get; set; }

        /// <summary>
        /// 完整配置信息
        /// </summary>
        public ComponentFullConfiguration? FullConfig { get; set; }
    }
}
