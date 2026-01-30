namespace PMCSystem_Backend.Dtos.PipeSpecInfo
{
    /// <summary>
    /// 标准文件配置
    /// </summary>
    public class StandardFileConfig
    {
        public object StandardFile {  get; set; }       // 接收 string 或 number

        public object Material { get; set; }        // 接收 string 或 number

        public double? MinNpdValue { get; set; }

        public double? MaxNpdValue { get; set; }

        public object BendRadiusMultiple { get; set; }      // string, number 或 null
    }
}
