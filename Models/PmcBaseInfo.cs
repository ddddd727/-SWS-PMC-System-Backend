namespace PMCSystem_Backend.Models
{
    public class PmcBaseInfo
    {
        // pmc7位编码
        public string PmcCode { get; set; }

        // 船号
        public string ShipNumber { get; set; }

        // pmc编码状态
        public string Status { get; set; }

        // 管材标准
        public string PipeStandard { get; set; }

        // 管材材料
        public string Material { get; set; }

        // 压力等级
        public string PressureClass { get; set; }

        // 壁厚系列
        public string WallThickness { get; set; }
    }
}
