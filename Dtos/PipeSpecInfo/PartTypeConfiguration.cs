using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecInfo
{
    public class PartTypeConfiguration
    {
        [Required(ErrorMessage = "部件类型不能为空")]
        public string PartType { get; set; }

        public string ConfigResult { get; set; }

        public FullConfiguration FullConfig { get; set; }
    }
}
