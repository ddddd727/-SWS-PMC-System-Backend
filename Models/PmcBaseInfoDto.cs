namespace PMCSystem_Backend.Models
{
    /// <summary>
    /// 7位编码PMC未进行附件添加时的基础信息
    /// </summary>
    public class PmcBaseInfoDto
    {
        // pmc7位编码
        public string PmcCode { get; set; }

        // 船号
        public string ShipNumber { get; set; }

        // pmc编码状态
        public string Status { get; set; }

        // 管道等级
        public string PipingClass { get; set; }

        // 牌号
        public string MaterialGrade { get; set; }

        // 法兰压力等级
        public string PressureRating { get; set; }

        // 管材标准
        public string PipeStandard { get; set; }

        // 管材材料
        public string MaterialCategory { get; set; }

        // 壁厚系列
        public string WallThickness { get; set; }
    }
}
