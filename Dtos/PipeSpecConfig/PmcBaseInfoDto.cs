namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 7位编码PMC未进行附件添加时的基础信息
    /// </summary>
    public class PmcBaseInfoDto
    {
        /// <summary>PMC 7位编码</summary>
        public required string PmcCode { get; set; }

        /// <summary>船号</summary>
        public string? ShipNumber { get; set; }

        /// <summary>PMC 编码状态</summary>
        public string? Status { get; set; }

        /// <summary>管道等级</summary>
        public string? PipingClass { get; set; }

        /// <summary>牌号</summary>
        public string? MaterialGrade { get; set; }

        /// <summary>法兰压力等级</summary>
        public string? PressureRating { get; set; }

        /// <summary>管材标准</summary>
        public string? PipeStandard { get; set; }

        /// <summary>管材材料</summary>
        public string? MaterialCategory { get; set; }

        /// <summary>壁厚系列</summary>
        public string? WallThickness { get; set; }
    }
}
