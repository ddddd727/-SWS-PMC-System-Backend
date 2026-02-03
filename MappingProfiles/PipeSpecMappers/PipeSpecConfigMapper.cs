using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Models;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Requests;

namespace PMCSystem_Backend.MappingProfiles.PipeSpecMappers
{
    /// <summary>
    /// 管系规格配置映射器
    /// 负责将请求DTO转换为内部使用的标准信息格式
    /// </summary>
    public class PipeSpecConfigMapper : IPipeSpecConfigMapper
    {
        /// <summary>
        /// 将部件类型配置列表转换为标准信息列表
        /// </summary>
        /// <param name="configurations">部件类型配置列表</param>
        /// <returns>标准信息列表</returns>
        public List<PmcStandardInfo> MapToStandardInfos(List<ComponentTypeConfiguration> configurations)
        {
            if (configurations == null || !configurations.Any())
            {
                return new List<PmcStandardInfo>();
            }

            var standardInfos = new List<PmcStandardInfo>();

            foreach (var config in configurations)
            {
                // 处理标准文件配置
                var configStandards = MapStandardFileConfigurations(config);
                standardInfos.AddRange(configStandards);

                // 处理重复范围默认配置
                var duplicateStandards = MapDuplicateRangeDefaults(config);
                standardInfos.AddRange(duplicateStandards);
            }

            return standardInfos;
        }

        /// <summary>
        /// 映射标准文件配置
        /// </summary>
        /// <param name="config">部件类型配置</param>
        /// <returns>标准信息列表</returns>
        private List<PmcStandardInfo> MapStandardFileConfigurations(ComponentTypeConfiguration config)
        {
            var standardInfos = new List<PmcStandardInfo>();

            if (config.FullConfig?.Configurations == null)
            {
                return standardInfos;
            }

            foreach (var stdConfig in config.FullConfig.Configurations)
            {
                var standardInfo = new PmcStandardInfo
                {
                    StandardType = config.ComponentType,
                    StandardName = stdConfig.StandardFileName,
                    Material = stdConfig.MaterialName,
                    DiameterRange = new DiameterRange
                    {
                        MinNpdValue = (double)stdConfig.NpdRange[0],
                        MaxNpdValue = (double)stdConfig.NpdRange[1],
                    },
                    IsDefault = false,
                    OverlapRange = null
                };

                standardInfos.Add(standardInfo);
            }

            return standardInfos;
        }

        /// <summary>
        /// 映射重复范围默认配置
        /// </summary>
        /// <param name="config">部件类型配置</param>
        /// <returns>标准信息列表</returns>
        private List<PmcStandardInfo> MapDuplicateRangeDefaults(ComponentTypeConfiguration config)
        {
            var standardInfos = new List<PmcStandardInfo>();

            if (config.FullConfig?.DuplicateRangeDefaults == null)
            {
                return standardInfos;
            }

            foreach (var duplicateDefault in config.FullConfig.DuplicateRangeDefaults)
            {
                var standardInfo = new PmcStandardInfo
                {
                    StandardType = config.ComponentType,
                    StandardName = duplicateDefault.DefaultStandardFileName,
                    Material = null, // 重复范围默认配置通常没有材料信息
                    DiameterRange = new DiameterRange
                    {
                        MinNpdValue = duplicateDefault.OverlapMin,
                        MaxNpdValue = duplicateDefault.OverlapMax
                    },
                    IsDefault = true,
                    OverlapRange = duplicateDefault.Ranges?.Select(r => new DiameterRange
                    {
                        MinNpdValue = r.MinNpdValue,
                        MaxNpdValue = r.MaxNpdValue,
                        StandardFile = r.StandardFile
                    }).ToList()
                };

                standardInfos.Add(standardInfo);
            }

            return standardInfos;
        }

        /// <summary>
        /// 验证保存请求的有效性
        /// </summary>
        /// <param name="request">保存请求</param>
        /// <returns>验证结果及错误消息</returns>
        public (bool IsValid, string ErrorMessage) ValidateRequest(SavePipeSpecRequest request)
        {
            if (request == null)
            {
                return (false, "保存请求不能为空");
            }

            if (string.IsNullOrWhiteSpace(request.PmcCode))
            {
                return (false, "PMC编码不能为空");
            }

            if (string.IsNullOrWhiteSpace(request.ShipType))
            {
                return (false, "船型不能为空");
            }

            if (string.IsNullOrWhiteSpace(request.ShipNumber))
            {
                return (false, "船号不能为空");
            }

            if (request.Configurations == null || !request.Configurations.Any())
            {
                return (false, "请至少配置一个部件类型");
            }

            // 验证每个配置的部件类型是否为空
            var invalidConfig = request.Configurations.FirstOrDefault(c => string.IsNullOrWhiteSpace(c.ComponentType));
            if (invalidConfig != null)
            {
                return (false, "部件类型不能为空");
            }

            return (true, string.Empty);
        }
    }
}
