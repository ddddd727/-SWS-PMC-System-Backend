namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Models
{
    /// <summary>
    /// 标准文件配置（简化版）
    /// </summary>
    public class StandardFileConfig
    {
        /// <summary>
        /// 标准文件ID（string 或 number）
        /// </summary>
        public object? StandardFile { get; set; }

        /// <summary>
        /// 材料ID（string 或 number）
        /// </summary>
        public object? Material { get; set; }

        /// <summary>
        /// 最小NPD值
        /// </summary>
        public double? MinNpdValue { get; set; }

        /// <summary>
        /// 最大NPD值
        /// </summary>
        public double? MaxNpdValue { get; set; }

        /// <summary>
        /// 弯管半径倍数（string, number 或 null）
        /// </summary>
        public object? BendRadiusMultiple { get; set; }
    }
}
