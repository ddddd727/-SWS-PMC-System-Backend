namespace PMCSystem_Backend.Models
{
    public class PMCCodeDto
    {
        // PMC7位编码
        public string PmcCode { get; set; }

        // 主材料
        public string Material { get; set; }

        // 压力等级
        public string PressureClass { get; set; }

        // 壁厚系列
        public string WallThickness { get; set; }

        // 管材等级
        public string PipingMaterialClass { get; set; }
    }
}
