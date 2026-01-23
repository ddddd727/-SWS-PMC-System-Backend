namespace PMCSystem_Backend.Dtos.PmcSpecRuleConfig
{
    public class SpecNPDInfoDto
    {
        // 端面标准（同几何工业标准）
        public string EndStandard { get; set; }

        // 壁厚系列
        public string Schedule { get; set; }

        // 通径范围
        public List<double> NPD { get; set; }

        // 外径范围
        public List<double> OutsideDiameter { get; set; }

        // 壁厚尺寸范围
        public List<double> WallThickness  { get; set; }
    }
}
