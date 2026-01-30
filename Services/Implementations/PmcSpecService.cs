using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class PmcSpecService : IPmcSpecService
    {
        private readonly PmcContext _context;

        // 暂时采用不同的PmcContext进行，防止冲突
        private readonly PmcContextCky _ckyContext;
        private readonly IMapper _mapper;
        private readonly ICodelistService _codelistService;
        private readonly ILogger<PmcSpecService> _logger;

        public PmcSpecService(PmcContext context, IMapper mapper, ICodelistService codelistService, ILogger<PmcSpecService> logger, PmcContextCky pmcContextCky)
        {
            _context = context;
            _mapper = mapper;
            _codelistService = codelistService;
            _logger = logger;
            _ckyContext = pmcContextCky;
        }

        /// <summary>
        /// 解析PMCcode内容，返回7位编码的解析结果
        /// </summary>
        /// <param name="PmcCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public PmcBaseInfoDto AnalyzeCodeFromPMC(string PmcCode)
        {
            if (string.IsNullOrWhiteSpace(PmcCode))
            {
                _logger.LogError("PMC编码不能为空");
                throw new ArgumentException("PMC编码不能为空");
            }

            // 确保为7位编码
            var singleCodes = PmcCode.ToArray();
            if (singleCodes.Length != 7)
            {
                _logger.LogError("输入的PMC编码不为7位: {PmcCode}", PmcCode);
                throw new Exception("输入的编码不为7位");
            }

            // 从数据库中查找对应PMC编码的数据
            var entity = _context.S3dRulePmcdata
                .AsNoTracking()
                .FirstOrDefault(x => x.Pmccode == PmcCode);

            if (entity == null)
            {
                _logger.LogError("未找到PMC编码 {PmcCode} 对应的数据", PmcCode);
                throw new Exception($"未找到PMC编码 {PmcCode} 对应的数据");
            }

            // 将实体数据映射到基础信息DTO
            var baseInfo = new PmcBaseInfoDto
            {
                PmcCode = entity.Pmccode,
                ShipNumber = entity.ShipNo,
                Status = entity.Status ?? string.Empty,
                PipingClass = entity.PipingClassName,
                MaterialGrade = entity.MaterialsGradeName,
                PressureRating = entity.PressureRatingName,
                PipeStandard = entity.PipingStandardName,
                MaterialCategory = entity.MaterialsCategoryName,
                WallThickness = entity.ScheduleThicknessName
            };

            return baseInfo;
        }

        /// <summary>
        /// 根据端面标准和壁厚系列获取通径、外径、壁厚信息
        /// </summary>
        /// <param name="EndStandard">端面标准</param>
        /// <param name="Schedule">壁厚系列</param>
        /// <returns>通径、外径、壁厚信息</returns>
        public SpecNPDInfoDto GetNPDInfoByPmc(string EndStandard, string Schedule)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(EndStandard) || string.IsNullOrWhiteSpace(Schedule))
            {
                _logger.LogError("端面标准和壁厚系列不能为空");
                throw new ArgumentException("端面标准和壁厚系列不能为空");
            }

            // 查询数据库中符合条件的数据
            //var queryResult = _context.S3dCommonPlainPipingGenericData
            //    .Where(x => x.EndStandardCl == endStandardCl && x.ScheduleCl == scheduleCl)
            //    .AsNoTracking()
            //    .ToList();

            // 更改当前的NPD通径范围的获取路径
            //var queryResult = _ckyContext.S3dCodeWallThicknesses
            //                    .Where(x => x.EndStandard == EndStandard && x.ScheduleThickness == Schedule)
            //                    .AsNoTracking()
            //                    .ToList();

            // 从S3d_WallThickness_Info视图中获取通径范围信息
            var queryResult = _context.S3dWallThicknessInfo
                                .Where(x => x.GeometricIndustryStandard == EndStandard && x.ScheduleThickness == Schedule)
                                .AsNoTracking()
                                .ToList();


            // 构建返回结果
            var result = new SpecNPDInfoDto
            {
                EndStandard = EndStandard,
                Schedule = Schedule,
                NPD = queryResult
                    .Where(x => x.NormalDiameter > 0)
                    .Select(x => (double)x.NormalDiameter)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                OutsideDiameter = queryResult
                    .Where(x => x.PipingOutsideDiameter > 0)
                    .Select(x => (double)x.PipingOutsideDiameter)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                WallThickness = queryResult
                    .Where(x => x.WallThickness > 0)
                    .Select(x => (double)x.WallThickness)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList()
            };

            return result;
        }

        public List<PipeFittingSpecDto> GetPipeFittingSpec(string compnentType)
        {
            if (string.IsNullOrWhiteSpace(compnentType))
            {
                _logger.LogError("compnentType 不能为空");
                throw new ArgumentException("compnentType 不能为空");
            }

            // 通过部件类型获取对应的 ComponentTypeId
            var compType = _context.S3dDictPipingComponentTypes
                .AsNoTracking()
                .FirstOrDefault(x => x.ComponentTypeName == compnentType);

            if (compType == null)
            {
                // 未找到对应的部件类型，返回空列表
                return new List<PipeFittingSpecDto>();
            }

            var componentTypeId = compType.Id;

            // 在标准表中查找该部件类型下的所有标准条目
            var standards = _context.S3dRulePipingCompStandards
                .AsNoTracking()
                .Where(x => x.ComponentTypeId == componentTypeId)
                .ToList();

            // 提取标准的 CodeList 值（假设存储在 GeometricIndustryStandardCl 字段）
            var codeValues = standards
                .Select(x => x.GeometricIndustryStandardCl)
                .Where(v => v > 0)
                .Distinct()
                .ToList();

            var result = new List<PipeFittingSpecDto>();

            foreach (var code in codeValues)
            {
                string standardName = code.ToString();

                // 先尝试通过表名+值的短描述方法
                try
                {
                    var shortDescTask = _codelistService.GetShortDesciptionByCodelistValue("GeometricIndustryStandard", code.ToString());
                    if (shortDescTask != null)
                    {
                        var tmp = shortDescTask.GetAwaiter().GetResult();
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            standardName = tmp;
                        }
                    }
                }
                catch
                {
                    // 忽略异常，尝试其他方式
                    _logger.LogWarning(code.ToString() + "无法通过表名+值的短描述方法获取标准名称");
                }

                // 若仍为 code 字符串，则尝试按列名查询描述
                if (standardName == code.ToString())
                {
                    try
                    {
                        var tmp = _codelistService.GetCodelistDescriptionAsync("GeometricIndustryStandard_Cl", code)
                            .GetAwaiter()
                            .GetResult();
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            standardName = tmp;
                        }
                    }
                    catch
                    {
                        _logger.LogWarning(code.ToString() + "无法通过列名查询描述方法获取标准名称");
                    }
                }

                // 如果查找均为将code转换为对应的短描述，则说明未成功查询到符合条件的标准
                if (standardName == code.ToString())
                {
                    return new List<PipeFittingSpecDto>();
                }

                // 通过 GetMaterialListByStandard 方法获取材料列表，不进行 commodityType 过滤
                List<string> materialList = new List<string>();
                

                try
                {
                    materialList = GetMaterialListByStandard(standardName, compnentType, string.Empty);
                }
                catch (Exception ex)
                {
                    // 如果获取材料列表失败，记录警告但继续处理，使用空列表
                    _logger.LogWarning(ex, "获取标准 {StandardName} 的材料列表失败", standardName);
                }

                result.Add(new PipeFittingSpecDto
                {
                    StandardName = standardName,
                    MaterialList = materialList
                });
            }

            return result.OrderBy(x => x.StandardName).ToList();
        }

        /// <summary>
        /// 根据船号获取PMC数据
        /// </summary>
        /// <param name="shipNumber">船号</param>
        /// <returns>PMC编码列表信息</returns>
        public List<PmcSelectInfoDto> GetPmcRulesByShipNum(string shipNumber)
        {
            // 根据船号查询数据库中的PMC数据
            var pmcDataList = _context.S3dRulePmcdata
                .Where(x => x.ShipNo == shipNumber)
                .AsNoTracking()
                .ToList();

            // 使用AutoMapper将实体类转换为DTO
            var result = _mapper.Map<List<PmcSelectInfoDto>>(pmcDataList);

            return result;
        }

        /// <summary>
        /// 获取所有船型船号信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ShipInfo> GetShipInfos()
        {
            // 从外部接口获取船型船号，目前先暂时用模拟数据代替
            List<ShipInfo> shipInfos = new List<ShipInfo>
            {
                new ShipInfo { shipNumber = "H1508", shipType = "邮轮" },
                new ShipInfo { shipNumber = "H1509", shipType = "邮轮" },
                new ShipInfo { shipNumber = "H1403", shipType = "民船" },
                new ShipInfo { shipNumber = "H1404", shipType = "民船" },
                new ShipInfo { shipNumber = "H1301", shipType = "货船" },
                new ShipInfo {shipNumber = "H1603", shipType = "民船" }
            };
            return shipInfos;
        }

        /// <summary>
        /// 获取所有部件类型信息
        /// </summary>
        /// <returns>部件类型列表</returns>
        public List<ComponentTypeInfoDto> GetComponentTypes()
        {
            var componentTypes = _context.S3dDictPipingComponentTypes
                .AsNoTracking()
                .Select(x => new ComponentTypeInfoDto
                {
                    ComponentTypeName = x.ComponentTypeName,
                    ComponentTypeDescription = x.ComponentTypeDescription
                })
                .OrderBy(x => x.ComponentTypeName)
                .ToList();

            return componentTypes;
        }


        /// <summary>
        /// 通过标准名称获取对应的材料列表
        /// </summary>
        /// <param name="standardName">几何工业标准名称</param>
        /// <param name="ComponentType">部件类型名称</param>
        /// <param name="commidityType">商品类型（可选过滤）</param>
        /// <returns>去重后的材料列表</returns>
        public List<string> GetMaterialListByStandard(string standardName, string ComponentType, string commidityType)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(standardName))
            {
                throw new ArgumentException("标准名称不能为空", nameof(standardName));
            }

            if (string.IsNullOrWhiteSpace(ComponentType))
            {
                throw new ArgumentException("部件类型不能为空", nameof(ComponentType));
            }

            // 1. 通过传入的ComponentType，到对应的部件类型表中获取到其ComponentTypeId
            var compType = _context.S3dDictPipingComponentTypes
                .AsNoTracking()
                .FirstOrDefault(x => x.ComponentTypeName == ComponentType);

            if (compType == null)
            {
                // 未找到对应的部件类型，返回空列表
                _logger.LogWarning("未找到对应的部件类型: {ComponentType}", ComponentType);
                return new List<string>();
            }

            var componentTypeId = compType.Id;

            // 2. 通过映射的ComponentTypeId找到S3D_Rule_ComponentTypeHierarchy中的映射的PipingCommoditySubClassCl
            var hierarchyRules = _context.S3dRuleComponentTypeHierarchyRules
                .AsNoTracking()
                .Where(x => x.ComponentTypeId == componentTypeId && x.Status)
                .ToList();

            if (!hierarchyRules.Any())
            {
                // 未找到层级规则，返回空列表
                _logger.LogWarning("未找到对应的层级规则 for ComponentTypeId: {ComponentTypeId}", componentTypeId);
                return new List<string>();
            }

            // 3. 通过SubClassCl和对应的codelist表找到对应子表中的具体的CommodityType类型
            var commodityTypes = new List<string>();

            foreach (var rule in hierarchyRules)
            {
                if (!rule.PipingCommoditySubClassCl.HasValue)
                {
                    continue;
                }

                var subClassCl = rule.PipingCommoditySubClassCl.Value;

                try
                {
                    // 获取子codelist表中的所有CommodityType值
                    var childCodeLists = _codelistService
                        .GetChildCodeListsByParentValueAsync("PipingCommoditySubClass", subClassCl)
                        .GetAwaiter()
                        .GetResult();

                    if (childCodeLists != null)
                    {
                        foreach (var kvp in childCodeLists)
                        {
                            foreach (var item in kvp.Value)
                            {
                                if (!string.IsNullOrWhiteSpace(item.ShortStringValue))
                                {
                                    commodityTypes.Add(item.ShortStringValue);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // 忽略异常，继续处理下一个规则
                    _logger.LogWarning("无法通过子类代码 {SubClassCl} 获取对应的 CommodityType 列表", subClassCl);
                }
            }

            // 如果传入了特定的commidityType，则仅使用该类型进行过滤
            if (!string.IsNullOrWhiteSpace(commidityType))
            {
                commodityTypes = commodityTypes
                    .Where(ct => ct.Equals(commidityType, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // 如果过滤后没有匹配项，直接使用传入的commidityType
                if (!commodityTypes.Any())
                {
                    commodityTypes.Add(commidityType);
                }
            }

            if (!commodityTypes.Any())
            {
                _logger.LogWarning("未找到任何匹配的 CommodityType for ComponentType: {ComponentType}", ComponentType);
                return new List<string>();
            }

            // 去重CommodityType列表
            commodityTypes = commodityTypes.Distinct().ToList();

            // 4. 通过输入的StandardName和CommodityType，到S3dCdbPipeComponent中查询材料
            var materials = _context.S3dCdbPipeComponents
                .AsNoTracking()
                .Where(x => x.GeometricIndustryStandard == standardName
                         && x.CommodityType != null
                         && commodityTypes.Contains(x.CommodityType)
                         && !string.IsNullOrEmpty(x.MaterialGrade))
                .Select(x => x.MaterialGrade!)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return materials;
        }


        /// <summary>
        /// 保存PMC管系规格书中的标准规格
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <param name="standardInfos">标准信息列表</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool SaveSpecRules(string pmcCode, List<PmcStandardInfo> standardInfos)
        {
            if (string.IsNullOrWhiteSpace(pmcCode))
            {
                _logger.LogError("PMC编码不能为空");
                throw new ArgumentException("PMC编码不能为空");
            }

            if (standardInfos == null || !standardInfos.Any())
            {
                _logger.LogError("标准信息列表不能为空");
                throw new ArgumentException("标准信息列表不能为空");
            }

            try
            {
                // 查询现有的PMC数据
                var existingEntity = _context.S3dRulePmcdata
                    .FirstOrDefault(x => x.Pmccode == pmcCode);

                if (existingEntity == null)
                {
                    _logger.LogError("未找到PMC编码 {PmcCode} 对应的数据，无法更新规则", pmcCode);
                    throw new Exception($"未找到PMC编码 {pmcCode} 对应的数据");
                }

                // 根据 StandardType 将标准信息分组
                var groupedStandards = standardInfos.GroupBy(x => x.StandardType);

                foreach (var group in groupedStandards)
                {
                    var standardType = group.Key;
                    var standards = group.ToList();

                    // 根据部件类型分配到对应的实体字段
                    switch (standardType)
                    {
                        case "Elbow":
                            // existingEntity.ElbowStandard = MergeStandardList(existingEntity.ElbowStandard, standards);
                            existingEntity.ElbowStandard = standards;
                            break;
                        case "Reducer":
                            existingEntity.RedStandard = standards;
                            break;
                        case "Tee":
                            existingEntity.TeeStandard = standards;
                            break;
                        case "Sleeve":
                            existingEntity.SleeveStandard = standards;
                            break;
                        case "Bosses":
                            existingEntity.BossesStandard = standards;
                            break;
                        case "Saddles":
                            existingEntity.SaddlesStandard = standards;
                            break;
                        case "Caps":
                            existingEntity.CapsStandard = standards;
                            break;
                        case "Overpass":
                            existingEntity.OverpassStandard = standards;
                            break;
                        case "Accessories":
                            existingEntity.AccessoriesStandard = standards;
                            break;
                        case "Flange":
                            existingEntity.FlangeStandard = standards;
                            break;
                        case "BlindFlange":
                            existingEntity.BlindFlangeStandard = standards;
                            break;
                        case "Gasket":
                            existingEntity.GasketStandard = standards;
                            break;
                        case "Bolt":
                            existingEntity.BoltStandard = standards;
                            break;
                        case "Nut":
                            existingEntity.NutStandard = standards;
                            break;
                        case "Washer":
                            existingEntity.WasherStandard = standards;
                            break;
                        case "Pipe":
                            existingEntity.PipeStandard = standards;
                            break;
                        default:
                            _logger.LogWarning("未知的部件类型: {StandardType}", standardType);
                            break;
                    }
                }

                // 更新状态为已配置
                existingEntity.Status = "已配置";

                // 保存更改
                _context.SaveChanges();

                _logger.LogInformation("成功保存PMC编码 {PmcCode} 的规格规则", pmcCode);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存PMC编码 {PmcCode} 的规格规则时发生错误", pmcCode);
                throw;
            }
        }

        /// <summary>
        /// 合并标准列表，将新规则追加到现有列表中
        /// </summary>
        /// <param name="existingList">现有的标准列表</param>
        /// <param name="newList">新的标准列表</param>
        /// <returns>合并后的标准列表</returns>
        private List<PmcStandardInfo> MergeStandardList(List<PmcStandardInfo>? existingList, List<PmcStandardInfo> newList)
        {
            if (existingList == null || !existingList.Any())
            {
                return newList.ToList();
            }

            var result = existingList.ToList();

            foreach (var newItem in newList)
            {
                // 检查是否已存在相同标准名称的配置
                var existingItem = result.FirstOrDefault(x =>
                    x.StandardName == newItem.StandardName);

                if (existingItem != null)
                {
                    // 更新现有配置
                    existingItem.DiameterRange = newItem.DiameterRange;
                    existingItem.Material = newItem.Material;
                    existingItem.IsDefault = newItem.IsDefault;
                    existingItem.OverlapRange = newItem.OverlapRange;
                }
                else
                {
                    // 添加新配置
                    result.Add(newItem);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据PmcCode获取对应的规格规则
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <param name="standardInfos">输出的标准信息列表</param>
        /// <returns>是否成功获取规则</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool GetSpecRules(string pmcCode, out List<PmcStandardInfo> standardInfos)
        {
            standardInfos = new List<PmcStandardInfo>();

            if (string.IsNullOrWhiteSpace(pmcCode))
            {
                _logger.LogError("PMC编码不能为空");
                throw new ArgumentException("PMC编码不能为空");
            }

            try
            {
                // 根据PMC编码查询数据库
                var entity = _context.S3dRulePmcdata
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Pmccode == pmcCode);

                if (entity == null)
                {
                    _logger.LogWarning("未找到PMC编码 {PmcCode} 对应的数据", pmcCode);
                    return false;
                }

                // 将实体中所有标准字段合并到一个列表中
                AddStandardsToList(standardInfos, entity.PipeStandard);
                AddStandardsToList(standardInfos, entity.ElbowStandard);
                AddStandardsToList(standardInfos, entity.RedStandard);
                AddStandardsToList(standardInfos, entity.TeeStandard);
                AddStandardsToList(standardInfos, entity.SleeveStandard);
                AddStandardsToList(standardInfos, entity.BossesStandard);
                AddStandardsToList(standardInfos, entity.SaddlesStandard);
                AddStandardsToList(standardInfos, entity.CapsStandard);
                AddStandardsToList(standardInfos, entity.OverpassStandard);
                AddStandardsToList(standardInfos, entity.AccessoriesStandard);
                AddStandardsToList(standardInfos, entity.FlangeStandard);
                AddStandardsToList(standardInfos, entity.BlindFlangeStandard);
                AddStandardsToList(standardInfos, entity.GasketStandard);
                AddStandardsToList(standardInfos, entity.BoltStandard);
                AddStandardsToList(standardInfos, entity.NutStandard);
                AddStandardsToList(standardInfos, entity.WasherStandard);

                _logger.LogInformation("成功获取PMC编码 {PmcCode} 的规格规则，共 {Count} 条", pmcCode, standardInfos.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取PMC编码 {PmcCode} 的规格规则时发生错误", pmcCode);
                throw;
            }
        }

        /// <summary>
        /// 将标准列表添加到目标列表中
        /// </summary>
        /// <param name="targetList">目标列表</param>
        /// <param name="sourceList">源列表</param>
        private void AddStandardsToList(List<PmcStandardInfo> targetList, List<PmcStandardInfo>? sourceList)
        {
            if (sourceList != null && sourceList.Any())
            {
                targetList.AddRange(sourceList);
            }
        }


        /// <summary>
        /// 生成对应的管系规格书
        /// </summary>
        /// <returns></returns>
        public bool GeneratePipeSpecTable()
        {
            throw new NotImplementedException();
        }
    }
}