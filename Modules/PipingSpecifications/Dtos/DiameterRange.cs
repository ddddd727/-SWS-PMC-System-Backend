namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos
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
        public string? StandardFile { get; set; }
    }
}
