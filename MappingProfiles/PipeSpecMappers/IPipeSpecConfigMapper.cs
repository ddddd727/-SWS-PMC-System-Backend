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
        /// 将部件类型配置列表转换为标准信息列表
        /// </summary>
        /// <param name="configurations">部件类型配置列表</param>
        /// <returns>标准信息列表</returns>
        List<PmcStandardInfo> MapToStandardInfos(List<ComponentTypeConfiguration> configurations);

        /// <summary>
        /// 验证保存请求的有效性
        /// </summary>
        /// <param name="request">保存请求</param>
        /// <returns>验证结果及错误消息</returns>
        (bool IsValid, string ErrorMessage) ValidateRequest(SavePipeSpecRequest request);
    }
}
