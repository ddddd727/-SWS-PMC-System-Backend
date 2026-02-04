namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 规格书中的PMC对应的通径配置信息
    /// </summary>
    public class SpecNPDInfoDto
    {
        /// <summary>端面标准（同几何工业标准）</summary>
        public string? EndStandard { get; set; }

        /// <summary>壁厚系列</summary>
        public string? Schedule { get; set; }

        /// <summary>通径范围</summary>
        public List<double>? NPD { get; set; }

        /// <summary>外径范围</summary>
        public List<double>? OutsideDiameter { get; set; }

        /// <summary>壁厚尺寸范围</summary>
        public List<double>? WallThickness { get; set; }
    }
}
