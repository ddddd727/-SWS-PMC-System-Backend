namespace PMCSystem_Backend.Entities.PipeSpecConfig
{
    /// <summary>
    ///  船型船号信息（模拟，最终信息会从DSP中接口获取）
    /// </summary>
    public class ShipInfo
    {
        // 船号
        public string? shipNumber { get; set; }

        // 船型
        public string? shipType {  get; set; }
    }
}
