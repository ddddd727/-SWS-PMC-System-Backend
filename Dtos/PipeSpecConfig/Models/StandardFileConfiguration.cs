using System.Text.Json;

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
        /// NPD范围 [最小NPD, 最大NPD]，支持 number[] 或 string[] 的 JSON 反序列化
        /// </summary>
        public object[]? NpdRange { get; set; }

        /// <summary>
        /// 弯管半径倍数（string, number 或 null）
        /// </summary>
        public object? BendRadiusMultiple { get; set; }

        /// <summary>
        /// 最小NPD值（辅助属性）
        /// </summary>
        public double MinNpdValue => SafeToDouble(NpdRange, 0) ?? 0;

        /// <summary>
        /// 最大NPD值（辅助属性）
        /// </summary>
        public double MaxNpdValue => SafeToDouble(NpdRange, 1) ?? 0;

        private static double? SafeToDouble(object[]? arr, int index)
        {
            if (arr == null || arr.Length <= index) return null;
            try
            {
                var v = arr[index];
                if (v is JsonElement je && je.ValueKind == JsonValueKind.Number)
                    return je.GetDouble();
                return Convert.ToDouble(v);
            }
            catch { return null; }
        }
    }
}
