using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecInfo
{
    /// <summary>
    /// 管系规格书保存数据传输对象
    /// </summary>
    public class PipeSpecSaveRequest
    {
        [Required(ErrorMessage = "船型不能为空")]
        public string ShipType { get; set; }

        [Required(ErrorMessage = "船号不能为空")]
        public string ShipNumber { get; set; }

        [Required(ErrorMessage ="PMC编码不能为空")]
        public string PmcCode { get; set; }

        [Required(ErrorMessage = "请至少配置一个部件类型")]
        [MinLength(1, ErrorMessage = "请至少配置一个部件类型")]
        public List<PartTypeConfiguration> Configurations { get; set; } = new List<PartTypeConfiguration>();
        
        // 可选元数据
        public Dictionary<string, object> Metadata { get; set; }
    }
}
