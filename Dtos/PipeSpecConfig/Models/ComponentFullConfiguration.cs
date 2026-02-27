namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 部件完整配置信息
    /// </summary>
    public class ComponentFullConfiguration
    {
        /// <summary>
        /// 标准文件ID列表
        /// </summary>
        public List<object> StandardFileIds { get; set; } = new List<object>();

        /// <summary>
        /// 标准文件配置列表
        /// </summary>
        public List<StandardFileConfig> StandardFileConfigs { get; set; } = new List<StandardFileConfig>();

        /// <summary>
        /// 重复范围默认配置列表
        /// </summary>
        public List<DuplicateRangeDefault> DuplicateRangeDefaults { get; set; } = new List<DuplicateRangeDefault>();
    }
}
