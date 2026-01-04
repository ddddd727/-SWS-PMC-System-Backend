namespace PMCSystem_Backend.Entities.PipeSpecConfig
{
    /// <summary>
    /// 弯管数据
    /// </summary>
    public class PipingBend
    {
        // 弯管外径
        public decimal? OutSideDiameter { get; set; }

        // 弯管外径单位
        public string? OutSideDiameterUnit { get; set; }
        
        // 头 clamp 长度
        public decimal? HeaderClampLength { get; set; }
        
        // 尾 clamp 长度
        public decimal? TailClampLength { get; set; }
    }
}
