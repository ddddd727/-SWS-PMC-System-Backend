using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// S3dRulePmcData 响应DTO
    /// </summary>
    public class S3dRulePmcDataDto
    {
        public int Id { get; set; }

        public string ShipType { get; set; } = null!;

        public string ShipNo { get; set; } = null!;

        public string Pmccode { get; set; } = null!;

        public string PipingClassName { get; set; } = null!;

        public string MaterialsCategoryName { get; set; } = null!;

        public List<PmcStandardInfo>? PipingStandardName { get; set; }

        public string? MaterialsGradeName { get; set; }

        public string? FlangeStandardName { get; set; }

        public string? PressureRatingName { get; set; }

        public string? ScheduleThicknessName { get; set; }

        public List<PmcStandardInfo>? PipeStandard { get; set; }

        public List<PmcStandardInfo>? ElbowStandard { get; set; }

        public List<PmcStandardInfo>? RedStandard { get; set; }

        public List<PmcStandardInfo>? TeeStandard { get; set; }

        public List<PmcStandardInfo>? SleeveStandard { get; set; }

        public List<PmcStandardInfo>? BossesStandard { get; set; }

        public List<PmcStandardInfo>? SaddlesStandard { get; set; }

        public List<PmcStandardInfo>? CapsStandard { get; set; }

        public List<PmcStandardInfo>? OverpassStandard { get; set; }

        public List<PmcStandardInfo>? AccessoriesStandard { get; set; }

        public List<PmcStandardInfo>? FlangeStandard { get; set; }

        public List<PmcStandardInfo>? BlindFlangeStandard { get; set; }

        public List<PmcStandardInfo>? GasketStandard { get; set; }

        public List<PmcStandardInfo>? BoltStandard { get; set; }

        public List<PmcStandardInfo>? NutStandard { get; set; }

        public List<PmcStandardInfo>? WasherStandard { get; set; }

        public string? Status { get; set; }

        public string? JsonData { get; set; }
    }

    /// <summary>
    /// 创建 S3dRulePmcData 请求DTO
    /// </summary>
    public class CreateS3dRulePmcDataDto
    {
        [Required(ErrorMessage = "船型不能为空")]
        [MaxLength(255, ErrorMessage = "船型长度不能超过255个字符")]
        public required string ShipType { get; set; }

        [Required(ErrorMessage = "船号不能为空")]
        [MaxLength(255, ErrorMessage = "船号长度不能超过255个字符")]
        public required string ShipNo { get; set; }

        [Required(ErrorMessage = "PMC编码不能为空")]
        [MaxLength(255, ErrorMessage = "PMC编码长度不能超过255个字符")]
        public required string Pmccode { get; set; }

        [Required(ErrorMessage = "管系类别名称不能为空")]
        [MaxLength(255, ErrorMessage = "管系类别名称长度不能超过255个字符")]
        public required string PipingClassName { get; set; }

        [Required(ErrorMessage = "材料类别名称不能为空")]
        [MaxLength(255, ErrorMessage = "材料类别名称长度不能超过255个字符")]
        public required string MaterialsCategoryName { get; set; }

        public List<PmcStandardInfo>? PipingStandardName { get; set; }

        [MaxLength(255, ErrorMessage = "材料等级名称长度不能超过255个字符")]
        public string? MaterialsGradeName { get; set; }

        [Required(ErrorMessage = "法兰标准名称不能为空")]
        [MaxLength(255, ErrorMessage = "法兰标准名称长度不能超过255个字符")]
        public string FlangeStandardName { get; set; }

        [MaxLength(255, ErrorMessage = "压力等级名称长度不能超过255个字符")]
        public string? PressureRatingName { get; set; }

        [MaxLength(255, ErrorMessage = "壁厚系列名称长度不能超过255个字符")]
        public string? ScheduleThicknessName { get; set; }

        public List<PmcStandardInfo>? PipeStandard { get; set; }

        public List<PmcStandardInfo>? ElbowStandard { get; set; }

        public List<PmcStandardInfo>? RedStandard { get; set; }

        public List<PmcStandardInfo>? TeeStandard { get; set; }

        public List<PmcStandardInfo>? SleeveStandard { get; set; }

        public List<PmcStandardInfo>? BossesStandard { get; set; }

        public List<PmcStandardInfo>? SaddlesStandard { get; set; }

        public List<PmcStandardInfo>? CapsStandard { get; set; }

        public List<PmcStandardInfo>? OverpassStandard { get; set; }

        public List<PmcStandardInfo>? AccessoriesStandard { get; set; }

        public List<PmcStandardInfo>? FlangeStandard { get; set; }

        public List<PmcStandardInfo>? BlindFlangeStandard { get; set; }

        public List<PmcStandardInfo>? GasketStandard { get; set; }

        public List<PmcStandardInfo>? BoltStandard { get; set; }

        public List<PmcStandardInfo>? NutStandard { get; set; }

        public List<PmcStandardInfo>? WasherStandard { get; set; }

        [MaxLength(100, ErrorMessage = "状态长度不能超过100个字符")]
        public string? Status { get; set; }

        public string? JsonData { get; set; }
    }

    /// <summary>
    /// 更新 S3dRulePmcData 请求DTO
    /// </summary>
    public class UpdateS3dRulePmcDataDto
    {
        [MaxLength(255, ErrorMessage = "船型长度不能超过255个字符")]
        public string? ShipType { get; set; }

        [MaxLength(255, ErrorMessage = "船号长度不能超过255个字符")]
        public string? ShipNo { get; set; }

        [MaxLength(255, ErrorMessage = "PMC编码长度不能超过255个字符")]
        public string? Pmccode { get; set; }

        [MaxLength(255, ErrorMessage = "管系类别名称长度不能超过255个字符")]
        public string? PipingClassName { get; set; }

        [MaxLength(255, ErrorMessage = "材料类别名称长度不能超过255个字符")]
        public string? MaterialsCategoryName { get; set; }


        public string? PipingStandardName { get; set; }

        [MaxLength(255, ErrorMessage = "材料等级名称长度不能超过255个字符")]
        public string? MaterialsGradeName { get; set; }

        [MaxLength(255, ErrorMessage = "法兰标准名称长度不能超过255个字符")]
        public string? FlangeStandardName { get; set; }

        [MaxLength(255, ErrorMessage = "压力等级名称长度不能超过255个字符")]
        public string? PressureRatingName { get; set; }

        [MaxLength(255, ErrorMessage = "壁厚系列名称长度不能超过255个字符")]
        public string? ScheduleThicknessName { get; set; }

        public List<PmcStandardInfo>? PipeStandard { get; set; }

        public List<PmcStandardInfo>? ElbowStandard { get; set; }

        public List<PmcStandardInfo>? RedStandard { get; set; }

        public List<PmcStandardInfo>? TeeStandard { get; set; }

        public List<PmcStandardInfo>? SleeveStandard { get; set; }

        public List<PmcStandardInfo>? BossesStandard { get; set; }

        public List<PmcStandardInfo>? SaddlesStandard { get; set; }

        public List<PmcStandardInfo>? CapsStandard { get; set; }

        public List<PmcStandardInfo>? OverpassStandard { get; set; }

        public List<PmcStandardInfo>? AccessoriesStandard { get; set; }

        public List<PmcStandardInfo>? FlangeStandard { get; set; }

        public List<PmcStandardInfo>? BlindFlangeStandard { get; set; }

        public List<PmcStandardInfo>? GasketStandard { get; set; }

        public List<PmcStandardInfo>? BoltStandard { get; set; }

        public List<PmcStandardInfo>? NutStandard { get; set; }

        public List<PmcStandardInfo>? WasherStandard { get; set; }

        [MaxLength(100, ErrorMessage = "状态长度不能超过100个字符")]
        public string? Status { get; set; }

        public string? JsonData { get; set; }
    }
}
