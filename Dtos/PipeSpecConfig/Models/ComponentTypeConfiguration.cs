using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 部件类型配置
    /// </summary>
    public class ComponentTypeConfiguration
    {
        /// <summary>部件类型 ID（S3D_Dict_PipingComponentType.ID），优先使用；与 ComponentType 二选一</summary>
        public int? ComponentTypeId { get; set; }

        /// <summary>部件类型名称（展示或兼容旧请求，保存时以 ComponentTypeId 为准；与 ComponentTypeId 二选一）</summary>
        public string? ComponentType { get; set; }

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
