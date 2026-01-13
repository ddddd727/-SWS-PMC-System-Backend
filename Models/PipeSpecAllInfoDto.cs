namespace PMCSystem_Backend.Models
{
    /// <summary>
    /// 管系规格书所有信息(包含BaseInfo和对应的规格书标准配置内容)DTO
    /// </summary>
    public class PipeSpecAllInfoDto
    {
        // 管系规格书基础信息（从PMC编码解析得到）
        public PmcBaseInfoDto BaseInfo { get; set; }

        // 管系规格书标准配置信息
        public PmcSpecInfoDto SpecInfo { get; set; }
    }
}
