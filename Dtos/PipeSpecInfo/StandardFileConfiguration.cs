using Microsoft.Identity.Client;

namespace PMCSystem_Backend.Dtos.PipeSpecInfo
{
    public class StandardFileConfiguration
    {
        public object StandardFileId { get; set; }      // 接收 string 或 number

        public string StandardFileName { get; set; }

        public object MaterialId { get; set; }

        public string MaterialName { get; set; }

        public object[] NpdRange { get; set; }  // [最小NPD，最大NPD]

        public object BendRadiusMultiple { get; set; }  // string, number 或 null

        // 辅助属性
        public double MinNpdValue => NpdRange?.Length > 0 ? double.Parse(NpdRange[0].ToString()) : 0;

        public double MaxNpdValue => NpdRange?.Length > 1 ? double.Parse(NpdRange[1].ToString()) : 0;
    }
}
