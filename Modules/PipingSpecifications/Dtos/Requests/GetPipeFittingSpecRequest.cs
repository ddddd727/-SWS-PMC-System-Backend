using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests
{
    /// <summary>
    /// 获取管附件标准请求（根据部件类型筛选）
    /// </summary>
    public class GetPipeFittingSpecRequest : IValidatableObject
    {
        /// <summary>
        /// 部件类型 ID（S3D_Dict_PipingComponentType.ID），推荐使用；与 ComponentTypeName 二选一
        /// </summary>
        public int? ComponentTypeId { get; set; }

        /// <summary>
        /// 部件类型名称（展示或兼容旧请求，长度≤255；与 ComponentTypeId 二选一）
        /// </summary>
        [MaxLength(255, ErrorMessage = "部件类型名称长度不能超过255个字符")]
        public string? ComponentTypeName { get; set; }

        /// <summary>
        /// 主材料名称（Codelist: MaterialsCategory 的 ShortStringValue），长度≤255；必填
        /// </summary>
        [MaxLength(255, ErrorMessage = "主材料名称长度不能超过255个字符")]
        public string? MaterialCategory { get; set; }

        /// <summary>
        /// 交叉字段校验：ComponentTypeId 与 ComponentTypeName 至少提供一个
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ComponentTypeId == null && string.IsNullOrWhiteSpace(ComponentTypeName))
            {
                yield return new ValidationResult(
                    "请至少提供 ComponentTypeId 或 ComponentTypeName 之一",
                    new[] { nameof(ComponentTypeId), nameof(ComponentTypeName) });
            }

            if (ComponentTypeId.HasValue && ComponentTypeId.Value <= 0)
            {
                yield return new ValidationResult(
                    "ComponentTypeId 必须为正整数",
                    new[] { nameof(ComponentTypeId) });
            }

            if (string.IsNullOrWhiteSpace(MaterialCategory))
            {
                yield return new ValidationResult(
                    "MaterialCategory 不能为空",
                    new[] { nameof(MaterialCategory) });
            }
        }
    }
}
