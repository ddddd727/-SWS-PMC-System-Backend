using PMCSystem_Backend.Dtos.PipeSpecConfig.Models;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// PMC编码信息（包含基础信息和配置信息）
    /// </summary>
    public class PmcInfoWithConfigDto
    {
        /// <summary>
        /// PMC基础信息
        /// </summary>
        public PmcBaseInfoDto BaseInfo { get; set; } = null!;

        /// <summary>
        /// 配置信息列表（如果已配置则包含，未配置则为空列表）
        /// </summary>
        public List<ComponentTypeConfiguration> Configurations { get; set; } = new List<ComponentTypeConfiguration>();

        /// <summary>
        /// 规格书配置状态：pending-待配置, review-待审核, approved-已审核
        /// </summary>
        public string ConfigStatus { get; set; } = "pending";

        /// <summary>
        /// 是否已配置
        /// </summary>
        public bool IsConfigured => Configurations != null && Configurations.Any();
    }
}
