using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Models;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Requests;

namespace PMCSystem_Backend.MappingProfiles.PipeSpecMappers
{
    /// <summary>
    /// 管系规格配置映射器接口
    /// </summary>
    public interface IPipeSpecConfigMapper
    {
        /// <summary>
        /// 将部件类型配置列表转换为标准信息列表（完整版，包含通径范围）
        /// </summary>
        /// <param name="configurations">部件类型配置列表</param>
        /// <returns>标准信息列表</returns>
        List<PmcStandardInfo> MapToStandardInfos(List<ComponentTypeConfiguration> configurations);

        /// <summary>
        /// 将简化的部件类型配置列表转换为标准信息列表（简化版，仅包含标准名称和材料）
        /// </summary>
        /// <param name="configurations">简化的部件类型配置列表</param>
        /// <returns>标准信息列表</returns>
        List<PmcStandardInfo> MapSimpleConfigurationsToStandardInfos(List<SimpleComponentTypeConfiguration> configurations);

        /// <summary>
        /// 验证保存请求的有效性（完整版）
        /// </summary>
        /// <param name="request">保存请求</param>
        /// <returns>验证结果及错误消息</returns>
        (bool IsValid, string ErrorMessage) ValidateRequest(SavePipeSpecRequest request);

        /// <summary>
        /// 验证简化保存请求的有效性
        /// </summary>
        /// <param name="request">简化保存请求</param>
        /// <returns>验证结果及错误消息</returns>
        (bool IsValid, string ErrorMessage) ValidateSimpleRequest(SavePipeSpecSimpleRequest request);
    }
}
