namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 标准文件配置（完整版）
    /// </summary>
    public class StandardFileConfiguration
    {
        /// <summary>
        /// 标准文件ID（string 或 number）
        /// </summary>
        public object? StandardFileId { get; set; }

        /// <summary>
        /// 标准文件名称
        /// </summary>
        public string? StandardFileName { get; set; }

        /// <summary>
        /// 材料ID（string 或 number）
        /// </summary>
        public object? MaterialId { get; set; }

        /// <summary>
        /// 材料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// NPD范围 [最小NPD, 最大NPD]
        /// </summary>
        public string[]? NpdRange { get; set; }

        /// <summary>
        /// 弯管半径倍数（string, number 或 null）
        /// </summary>
        public object? BendRadiusMultiple { get; set; }

        /// <summary>
        /// 最小NPD值（辅助属性）
        /// </summary>
        public double MinNpdValue => NpdRange?.Length > 0 ? Convert.ToDouble(NpdRange[0]) : 0;

        /// <summary>
        /// 最大NPD值（辅助属性）
        /// </summary>
        public double MaxNpdValue => NpdRange?.Length > 1 ? Convert.ToDouble(NpdRange[1]) : 0;
    }
}
