using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 保存规格书配置请求
    /// </summary>
    public class SaveSpecRulesRequest
    {
        /// <summary>
        /// PMC编码
        /// </summary>
        [Required(ErrorMessage = "PMC编码不能为空")]
        public required string PmcCode { get; set; }

        /// <summary>
        /// 标准信息列表
        /// </summary>
        [Required(ErrorMessage = "标准信息列表不能为空")]
        public required List<PmcStandardInfo> StandardInfos { get; set; }
    }
}
