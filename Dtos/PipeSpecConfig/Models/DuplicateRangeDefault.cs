namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 重复NPD范围的默认配置
    /// </summary>
    public class DuplicateRangeDefault
    {
        /// <summary>
        /// 重叠范围最小值
        /// </summary>
        public double OverlapMin { get; set; }

        /// <summary>
        /// 重叠范围最大值
        /// </summary>
        public double OverlapMax { get; set; }

        /// <summary>
        /// 默认标准文件ID
        /// </summary>
        public object? DefaultStandardFileId { get; set; }

        /// <summary>
        /// 默认标准文件名称
        /// </summary>
        public string? DefaultStandardFileName { get; set; }

        /// <summary>
        /// NPD范围列表
        /// </summary>
        public List<DiameterRange> Ranges { get; set; } = new List<DiameterRange>();

        /// <summary>
        /// 标准文件列表
        /// </summary>
        public List<object> StandardFiles { get; set; } = new List<object>();

        /// <summary>
        /// 范围键
        /// </summary>
        public string? RangeKey { get; set; }
    }
}
