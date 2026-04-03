namespace PMCSystem_Backend.Modules.PMCRuleConfig.Dtos
{
    /// <summary>
    /// 用于根据船号选择对应PMC编码的数据传输
    /// </summary>
    public class PmcSelectInfoDto
    {
        // PMC 7位编码
        public string PmcCode { get; set; }

        // 船号
        public string ShipNumber { get; set; }

        // 主材料
        public string Material { get; set; }

        // 管材标准
        public string PipeStandard { get; set; }

        // PMC 编码状态
        public string status { get; set; }
    }
}
