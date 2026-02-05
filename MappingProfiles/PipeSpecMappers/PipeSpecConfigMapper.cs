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
        /// 映射标准文件配置（StandardFileConfigs）
        /// </summary>
        /// <param name="config">部件类型配置</param>
        /// <returns>标准信息列表</returns>
        private List<PmcStandardInfo> MapStandardFileConfigurations(ComponentTypeConfiguration config)
        {
            var standardInfos = new List<PmcStandardInfo>();

            if (config.FullConfig?.StandardFileConfigs != null)
            {
                foreach (var stdConfig in config.FullConfig.StandardFileConfigs)
                {
                    DiameterRange? range = null;
                    if (stdConfig.MinNpdValue != null && stdConfig.MaxNpdValue != null)
                    {
                        range = new DiameterRange
                        {
                            MinNpdValue = stdConfig.MinNpdValue.Value,
                            MaxNpdValue = stdConfig.MaxNpdValue.Value
                        };
                    }

                    standardInfos.Add(new PmcStandardInfo
                    {
                        StandardType = NormalizeComponentType(config.ComponentType),
                        StandardName = stdConfig.StandardFile?.ToString() ?? string.Empty,
                        Material = stdConfig.Material?.ToString(),
                        DiameterRange = range,
                        IsDefault = false,
                        OverlapRange = null
                    });
                }
            }

            return standardInfos;
        }

        /// <summary>
        /// 规范化部件类型名称，确保与 S3dRulePmcData 实体字段映射一致
        /// </summary>
        private static string NormalizeComponentType(string componentType)
        {
            if (string.IsNullOrWhiteSpace(componentType))
                return string.Empty;

            // 保持首字母大写，其余小写，与 PmcSpecService.SaveSpecRules 中的 switch 匹配
            return char.ToUpperInvariant(componentType[0]) + componentType[1..].ToLowerInvariant();
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
                    StandardType = NormalizeComponentType(config.ComponentType),
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
        /// 将简化的部件类型配置列表转换为标准信息列表（简化版：仅包含标准名称和材料，不包含通径范围）
        /// </summary>
        /// <param name="configurations">简化的部件类型配置列表</param>
        /// <returns>标准信息列表</returns>
        public List<PmcStandardInfo> MapSimpleConfigurationsToStandardInfos(List<SimpleComponentTypeConfiguration> configurations)
        {
            if (configurations == null || !configurations.Any())
            {
                return new List<PmcStandardInfo>();
            }

            var standardInfos = new List<PmcStandardInfo>();

            foreach (var config in configurations)
            {
                if (config.Standards == null || !config.Standards.Any())
                {
                    continue;
                }

                foreach (var standard in config.Standards)
                {
                    // 只提取标准名称和材料信息，不包含通径范围
                    standardInfos.Add(new PmcStandardInfo
                    {
                        StandardType = NormalizeComponentType(config.ComponentType),
                        StandardName = standard.StandardFile?.ToString() ?? string.Empty,
                        Material = standard.Material?.ToString(),
                        DiameterRange = null, // 简化版不包含通径范围
                        IsDefault = false,
                        OverlapRange = null // 简化版不包含重复范围配置
                    });
                }
            }

            return standardInfos;
        }

        /// <summary>
        /// 验证保存请求的有效性（完整版）
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

        /// <summary>
        /// 验证简化保存请求的有效性
        /// </summary>
        /// <param name="request">简化保存请求</param>
        /// <returns>验证结果及错误消息</returns>
        public (bool IsValid, string ErrorMessage) ValidateSimpleRequest(SavePipeSpecSimpleRequest request)
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

            // 验证每个配置是否至少包含一个标准
            var configWithoutStandards = request.Configurations.FirstOrDefault(c =>
                c.Standards == null || !c.Standards.Any());
            if (configWithoutStandards != null)
            {
                return (false, $"部件类型 {configWithoutStandards.ComponentType} 必须至少配置一个标准");
            }

            // 验证每个标准是否包含必填字段
            foreach (var config in request.Configurations)
            {
                var invalidStandard = config.Standards.FirstOrDefault(s =>
                    s.StandardFile == null || s.Material == null);
                if (invalidStandard != null)
                {
                    return (false, $"部件类型 {config.ComponentType} 的标准配置中，标准文件和材料不能为空");
                }
            }

            return (true, string.Empty);
        }
    }
}
