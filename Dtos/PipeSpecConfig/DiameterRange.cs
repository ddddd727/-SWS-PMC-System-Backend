namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 通径范围（NPD Range）
    /// </summary>
    public class DiameterRange
    {
        /// <summary>
        /// 最小通径值
        /// </summary>
        public double MinNpdValue { get; set; }

        /// <summary>
        /// 最大通径值
        /// </summary>
        public double MaxNpdValue { get; set; }

        /// <summary>
        /// 通径单位
        /// </summary>
        public string? DiameterUnit { get; set; } = "mm";

        /// <summary>
        /// 标准文件（可选）
        /// </summary>
        public object? StandardFile { get; set; }

        // 保留向后兼容的属性
        /// <summary>
        /// 最小通径（兼容旧字段）
        /// </summary>
        public double DiameterMin
        {
            get => MinNpdValue;
            set => MinNpdValue = value;
        }

        /// <summary>
        /// 最大通径（兼容旧字段）
        /// </summary>
        public double DiameterMax
        {
            get => MaxNpdValue;
            set => MaxNpdValue = value;
        }
    }
}
