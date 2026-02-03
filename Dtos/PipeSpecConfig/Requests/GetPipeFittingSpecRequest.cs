using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig.Requests
{
    /// <summary>
    /// 获取管附件规格请求
    /// </summary>
    public class GetPipeFittingSpecRequest
    {
        /// <summary>
        /// 部件类型名称
        /// </summary>
        [Required(ErrorMessage = "部件类型名称不能为空")]
        public string ComponentTypeName { get; set; } = string.Empty;
    }
}
