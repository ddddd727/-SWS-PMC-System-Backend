using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.MappingProfiles.PipeSpecMappers;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Models;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared.Constants;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PMCSystem_Backend.Services.Implementations
{
    public class PmcSpecService : IPmcSpecService
    {
        private readonly AppDbContext _context;

        /// <summary>规范部件类型名 -> 实体标准属性 setter（用于按 ComponentTypeId 分发）</summary>
        private static readonly Dictionary<string, Action<S3dRulePmcData, List<PmcStandardInfo>>> ComponentTypeSetters =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Elbow"] = (e, list) => e.ElbowStandard = list,
                ["Reducer"] = (e, list) => e.RedStandard = list,
                ["Tee"] = (e, list) => e.TeeStandard = list,
                ["Sleeve"] = (e, list) => e.SleeveStandard = list,
                ["Bosses"] = (e, list) => e.BossesStandard = list,
                ["Saddles"] = (e, list) => e.SaddlesStandard = list,
                ["Caps"] = (e, list) => e.CapsStandard = list,
                ["Overpass"] = (e, list) => e.OverpassStandard = list,
                ["Accessories"] = (e, list) => e.AccessoriesStandard = list,
                ["Flange"] = (e, list) => e.FlangeStandard = list,
                ["BlindFlange"] = (e, list) => e.BlindFlangeStandard = list,
                ["Gasket"] = (e, list) => e.GasketStandard = list,
                ["Bolt"] = (e, list) => e.BoltStandard = list,
                ["Nut"] = (e, list) => e.NutStandard = list,
                ["Washer"] = (e, list) => e.WasherStandard = list,
                ["Pipe"] = (e, list) => e.PipeStandard = list,
            };
        private readonly IMapper _mapper;
        private readonly ICodelistService _codelistService;
        private readonly ILogger<PmcSpecService> _logger;
        private readonly IPipeSpecConfigMapper _pipeSpecConfigMapper;
        private readonly IPipeSpecVersionService _pipeSpecVersionService;

        public PmcSpecService(
            AppDbContext context,
            IMapper mapper,
            ICodelistService codelistService,
            ILogger<PmcSpecService> logger,
            IPipeSpecConfigMapper pipeSpecConfigMapper,
            IPipeSpecVersionService pipeSpecVersionService)
        {
            _context = context;
            _mapper = mapper;
            _codelistService = codelistService;
            _logger = logger;
            _pipeSpecConfigMapper = pipeSpecConfigMapper;
            _pipeSpecVersionService = pipeSpecVersionService;
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
        public async Task<SpecNPDInfoDto> GetNPDInfoByPmcAsync(string EndStandard, string Schedule)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(EndStandard) || string.IsNullOrWhiteSpace(Schedule))
            {
                _logger.LogError("端面标准和壁厚系列不能为空");
                throw new ArgumentException("端面标准和壁厚系列不能为空");
            }

            var endStandardCl = await _codelistService
                .GetCodeListNumberByShortDescriptionAsync("EndStandard", EndStandard);
            if (!endStandardCl.HasValue)
            {
                _logger.LogWarning("无法根据端面标准反查Codelist值，EndStandard: {EndStandard}", EndStandard);
                throw new ArgumentException("端面标准无效或未配置");
            }

            var scheduleThicknessCl = await _codelistService
                .GetCodeListNumberByShortDescriptionAsync("ScheduleThickness", Schedule);
            if (!scheduleThicknessCl.HasValue)
            {
                _logger.LogWarning("无法根据壁厚系列反查Codelist值，Schedule: {Schedule}", Schedule);
                throw new ArgumentException("壁厚系列无效或未配置");
            }

            var queryResult = await _context.S3dCommonPlainPipingGenericData
                .Where(x => x.EndStandardCl == endStandardCl.Value
                         && x.ScheduleThicknessCl == scheduleThicknessCl.Value
                         && x.Status)
                .AsNoTracking()
                .ToListAsync();

            // 构建返回结果
            var result = new SpecNPDInfoDto
            {
                EndStandard = EndStandard,
                Schedule = Schedule,
                NPD = queryResult
                    .Where(x => x.NominalPipingDiameter > 0)
                    .Select(x => x.NominalPipingDiameter)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                OutsideDiameter = queryResult
                    .Select(x => ParsePositiveDoubleOrNull(x.PipingOutsideDiameter))
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                WallThickness = queryResult
                    .Select(x => ParsePositiveDoubleOrNull(x.WallThickness))
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList()
            };

            return result;
        }

        /// <summary>
        /// 将 S3D_Common_PlainPipingGenericData 中外径/壁厚字段的字符串解析为可用于返回前端的正数。
        /// 兼容：纯数字、带单位后缀、千分位、欧式小数逗号、全角数字、文本中嵌入的数值等。
        /// </summary>
        /// <param name="rawValue">原始字符串值</param>
        /// <returns>解析成功且大于 0 的数值；否则 null</returns>
        private static double? ParsePositiveDoubleOrNull(string? rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return null;
            }

            var normalized = NormalizeEngineeringNumericString(rawValue);
            if (string.IsNullOrEmpty(normalized))
            {
                return null;
            }

            if (TryParseDoubleFlexible(normalized, out var value) && value > 0)
            {
                return value;
            }

            // 从较长描述中提取第一个数值片段（如 "OD 21.3 mm"、"Φ21.3"）
            var match = NumberTokenRegex.Match(normalized);
            if (match.Success && TryParseDoubleFlexible(match.Value, out value) && value > 0)
            {
                return value;
            }

            return null;
        }

        /// <summary>匹配工程类文本中的第一个浮点数字面量。</summary>
        private static readonly Regex NumberTokenRegex = new(
            @"[-+]?(?:\d+\.?\d*|\d*\.?\d+)(?:[eE][-+]?\d+)?",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// 去除不可见字符、全角数字转半角、去掉常见单位后缀，便于解析。
        /// </summary>
        private static string NormalizeEngineeringNumericString(string raw)
        {
            var sb = new StringBuilder(raw.Length);
            foreach (var c in raw.Trim())
            {
                if (c is >= '０' and <= '９')
                {
                    sb.Append((char)(c - '０' + '0'));
                }
                else if (c == '\u00A0' || c == '\u3000')
                {
                    sb.Append(' ');
                }
                else if (!char.IsControl(c))
                {
                    sb.Append(c);
                }
            }

            var s = sb.ToString().Trim();
            if (s.Length == 0)
            {
                return string.Empty;
            }

            // 去掉尾部常见单位（不区分大小写），如 "21.3 mm"、"21.3mm"
            s = Regex.Replace(
                s,
                @"\s*(mm|cm|m|in|inch|''|""|DN|Φ|φ|OD|ID)\s*$",
                "",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            return s.Trim();
        }

        /// <summary>
        /// 在多种小数/千分位规则下尝试解析为 double。
        /// </summary>
        private static bool TryParseDoubleFlexible(string s, out double value)
        {
            value = default;

            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            s = s.Trim();

            // 直接尝试
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            if (double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            {
                return true;
            }

            var dot = s.IndexOf('.');
            var comma = s.IndexOf(',');

            // 仅含逗号且视为欧式小数：21,3
            if (comma >= 0 && dot < 0 && s.IndexOf(',', comma + 1) < 0)
            {
                var eu = s.Replace(',', '.');
                if (double.TryParse(eu, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    return true;
                }
            }

            // 美式千分位 1,234.56 → 去掉逗号
            if (dot >= 0 && comma >= 0)
            {
                var us = s.Replace(",", "");
                if (double.TryParse(us, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    return true;
                }
            }

            // 欧式千分位 1.234,56 → 去点保留逗号再转
            if (comma > dot && dot >= 0)
            {
                var de = s.Replace(".", "").Replace(',', '.');
                if (double.TryParse(de, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetPipeFittingSpec(int? componentTypeId, string? componentTypeName)
        {
            if (componentTypeId == null && string.IsNullOrWhiteSpace(componentTypeName))
            {
                _logger.LogError("部件类型不能为空");
                throw new ArgumentException("部件类型不能为空");
            }

            int resolvedComponentTypeId;

            if (componentTypeId.HasValue)
            {
                resolvedComponentTypeId = componentTypeId.Value;
            }
            else
            {
                var normalizedName = componentTypeName?.Trim();
                if (string.IsNullOrWhiteSpace(normalizedName))
                {
                    _logger.LogError("部件类型名称不能为空");
                    throw new ArgumentException("部件类型名称不能为空");
                }

                var compType = _context.S3dDictPipingComponentTypes
                    .AsNoTracking()
                    .FirstOrDefault(x => x.ComponentTypeName == normalizedName);

                if (compType == null)
                {
                    // 未找到对应的部件类型，返回空列表
                    _logger.LogWarning("未找到对应的部件类型: {ComponentTypeName}", normalizedName);
                    return new List<string>();
                }

                resolvedComponentTypeId = compType.Id;
            }

            var standards = _context.S3dRulePipingCompStandards
                .AsNoTracking()
                .Where(x => x.ComponentTypeId == resolvedComponentTypeId)
                .ToList();

            var codeValues = standards
                .Select(x => x.GeometricIndustryStandardCl)
                .Where(v => v > 0)
                .Distinct()
                .ToList();

            var standardNames = new List<string>();

            foreach (var code in codeValues)
            {
                string standardName = code.ToString();

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
                    _logger.LogWarning("{Code} 无法通过表名+值的短描述方法获取标准名称", code);
                }

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
                        _logger.LogWarning("{Code} 无法通过列名查询描述方法获取标准名称", code);
                    }
                }

                if (standardName == code.ToString())
                {
                    return new List<string>();
                }

                standardNames.Add(standardName);
            }

            return standardNames
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public List<string> GetMaterialsGrades()
        {
            var grades = _context.S3dClMaterialsGrades
                .AsNoTracking()
                .Select(x => x.ShortStringValue)
                .Where(x => x != null && x != string.Empty)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return grades!;
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
                    Id = x.Id,
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
        /// 保存PMC管系规格书中的标准规格（简化版：仅包含标准名称和材料信息）
        /// </summary>
        /// <param name="request">简化的管系规格书保存请求</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool SaveSpecRulesSimple(SavePipeSpecSimpleRequest request)
        {
            // 使用 Mapper 对简化请求做基础校验（必填字段、至少一个部件类型与标准）
            var (isValid, errorMessage) = _pipeSpecConfigMapper.ValidateSimpleRequest(request);
            if (!isValid)
            {
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            try
            {
                // 查询现有的PMC数据：按 (Pmccode, ShipType, ShipNo) 精确匹配
                var existingEntity = _context.S3dRulePmcdata
                    .FirstOrDefault(x => x.Pmccode == request.PmcCode
                        && x.ShipType == request.ShipType
                        && x.ShipNo == request.ShipNumber);

                if (existingEntity == null)
                {
                    _logger.LogError("未找到PMC编码 {PmcCode} 船型 {ShipType} 船号 {ShipNo} 对应的数据，无法更新规则",
                        request.PmcCode, request.ShipType, request.ShipNumber);
                    throw new Exception($"未找到PMC编码 {request.PmcCode}（船型: {request.ShipType}, 船号: {request.ShipNumber}）对应的数据");
                }

                // 更新主表前，将当前配置保存为历史版本快照
                _pipeSpecVersionService.CreateVersionSnapshot(existingEntity);

                // 将前端的简化配置（仅标准+材料）转换为内部统一的标准信息列表
                var standardInfos = _pipeSpecConfigMapper.MapSimpleConfigurationsToStandardInfos(request.Configurations);

                // 兼容仅传 componentType 的旧请求：根据 StandardType 从字典表反查 ComponentTypeId
                ResolveComponentTypeIds(standardInfos);

                // 仅保留成功解析出 ComponentTypeId 的配置，避免依赖英文描述字符串
                var withId = standardInfos.Where(x => x.ComponentTypeId.HasValue && x.ComponentTypeId.Value > 0).ToList();
                var skipped = standardInfos.Count - withId.Count;
                if (skipped > 0)
                {
                    _logger.LogWarning("简化保存中有 {Count} 条标准配置未解析出 ComponentTypeId，已跳过；建议请求中传入 ComponentTypeId", skipped);
                }

                // 按部件类型 ID 分组，并通过预先定义的映射写入 S3dRulePmcData 对应字段
                var groupedByTypeId = withId.GroupBy(x => x.ComponentTypeId!.Value);
                var idToSetter = BuildComponentTypeIdToSetter();

                foreach (var group in groupedByTypeId)
                {
                    var typeId = group.Key;
                    var standards = group.ToList();
                    if (idToSetter.TryGetValue(typeId, out var setter))
                    {
                        setter(existingEntity, standards);
                    }
                    else
                    {
                        _logger.LogWarning("简化保存中未找到部件类型 ID {ComponentTypeId} 对应的实体字段，已跳过", typeId);
                    }
                }

                // 更新船型和船号信息
                existingEntity.ShipType = request.ShipType;
                existingEntity.ShipNo = request.ShipNumber;

                // 更新规格书配置状态为待审核
                existingEntity.Status = SpecConfigStatus.Review;

                // 保存更改
                _context.SaveChanges();

                _logger.LogInformation("成功保存PMC编码 {PmcCode} 的简化规格规则（仅标准+材料）", request.PmcCode);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存PMC编码 {PmcCode} 的简化规格规则时发生错误", request.PmcCode);
                throw;
            }
        }

        /// <summary>
        /// 保存PMC管系规格书中的标准规格（完整版：包含通径范围，保留给后续模块使用）
        /// </summary>
        /// <param name="request">管系规格书保存请求</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool SaveSpecRules(SavePipeSpecRequest request)
        {
            // 使用Mapper进行验证
            var (isValid, errorMessage) = _pipeSpecConfigMapper.ValidateRequest(request);
            if (!isValid)
            {
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            try
            {
                // 查询现有的PMC数据：按 (Pmccode, ShipType, ShipNo) 精确匹配，与 S3D_Rule_PMCData 唯一约束一致
                var existingEntity = _context.S3dRulePmcdata
                    .FirstOrDefault(x => x.Pmccode == request.PmcCode
                        && x.ShipType == request.ShipType
                        && x.ShipNo == request.ShipNumber);

                if (existingEntity == null)
                {
                    _logger.LogError("未找到PMC编码 {PmcCode} 船型 {ShipType} 船号 {ShipNo} 对应的数据，无法更新规则",
                        request.PmcCode, request.ShipType, request.ShipNumber);
                    throw new Exception($"未找到PMC编码 {request.PmcCode}（船型: {request.ShipType}, 船号: {request.ShipNumber}）对应的数据");
                }

                // 更新主表前，将当前配置保存为历史版本快照
                _pipeSpecVersionService.CreateVersionSnapshot(existingEntity);

                // 使用Mapper转换DTO结构为标准信息列表格式
                var standardInfos = _pipeSpecConfigMapper.MapToStandardInfos(request.Configurations);

                // 未传 ComponentTypeId 时按 StandardType 从字典表解析出 Id（兼容旧请求）
                ResolveComponentTypeIds(standardInfos);

                var withId = standardInfos.Where(x => x.ComponentTypeId.HasValue && x.ComponentTypeId.Value > 0).ToList();
                var skipped = standardInfos.Count - withId.Count;
                if (skipped > 0)
                    _logger.LogWarning("有 {Count} 条标准配置未解析出 ComponentTypeId，已跳过；建议请求中传入 ComponentTypeId", skipped);

                // 按 ComponentTypeId 分组，避免依赖英文描述
                var groupedByTypeId = withId.GroupBy(x => x.ComponentTypeId!.Value);

                var idToSetter = BuildComponentTypeIdToSetter();

                foreach (var group in groupedByTypeId)
                {
                    var typeId = group.Key;
                    var standards = group.ToList();
                    if (idToSetter.TryGetValue(typeId, out var setter))
                    {
                        setter(existingEntity, standards);
                    }
                    else
                    {
                        _logger.LogWarning("未找到部件类型 ID {ComponentTypeId} 对应的实体字段，已跳过", typeId);
                    }
                }

                // 更新船型和船号信息
                existingEntity.ShipType = request.ShipType;
                existingEntity.ShipNo = request.ShipNumber;

                // 更新规格书配置状态为待审核
                existingEntity.Status = SpecConfigStatus.Review;

                // 保存更改
                _context.SaveChanges();

                _logger.LogInformation("成功保存PMC编码 {PmcCode} 的规格规则", request.PmcCode);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存PMC编码 {PmcCode} 的规格规则时发生错误", request.PmcCode);
                throw;
            }
        }

        /// <summary>对未设置 ComponentTypeId 的项，按 StandardType 从字典表解析 Id。</summary>
        private void ResolveComponentTypeIds(List<PmcStandardInfo> standardInfos)
        {
            var needResolve = standardInfos.Where(x => !x.ComponentTypeId.HasValue || x.ComponentTypeId.Value <= 0).ToList();
            if (needResolve.Count == 0) return;

            var componentTypes = _context.S3dDictPipingComponentTypes.AsNoTracking().ToList();
            var nameToId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var ct in componentTypes)
            {
                var canonical = NormalizeComponentTypeNameToCanonical(ct.ComponentTypeName);
                if (!string.IsNullOrEmpty(canonical) && !nameToId.ContainsKey(canonical))
                    nameToId[canonical] = ct.Id;
            }

            foreach (var info in needResolve)
            {
                if (string.IsNullOrWhiteSpace(info.StandardType)) continue;
                var canonical = NormalizeComponentTypeNameToCanonical(info.StandardType);
                if (!string.IsNullOrEmpty(canonical) && nameToId.TryGetValue(canonical, out var id))
                    info.ComponentTypeId = id;
            }
        }

        /// <summary>将部件类型名称规范化为与 ComponentTypeSetters 一致的键（不区分大小写、去尾 s 等）。</summary>
        private static string? NormalizeComponentTypeNameToCanonical(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var t = name.Trim();
            if (t.Length == 0) return null;
            var lower = t.ToLowerInvariant();
            if (ComponentTypeSetters.ContainsKey(lower)) return lower;
            if (lower.EndsWith('s') && t.Length > 1 && ComponentTypeSetters.ContainsKey(lower[..^1]))
                return lower[..^1];
            if (lower.Replace(" ", "") == "blindflange" || lower.Contains("blind flange"))
                return "blindflange";
            return lower;
        }

        /// <summary>根据字典表构建 ComponentTypeId -> setter，用于按 ID 写入实体字段。</summary>
        private Dictionary<int, Action<S3dRulePmcData, List<PmcStandardInfo>>> BuildComponentTypeIdToSetter()
        {
            var componentTypes = _context.S3dDictPipingComponentTypes.AsNoTracking().ToList();
            var result = new Dictionary<int, Action<S3dRulePmcData, List<PmcStandardInfo>>>();
            foreach (var ct in componentTypes)
            {
                var canonical = NormalizeComponentTypeNameToCanonical(ct.ComponentTypeName);
                if (string.IsNullOrEmpty(canonical)) continue;
                if (ComponentTypeSetters.TryGetValue(canonical, out var setter))
                    result[ct.Id] = setter;
            }
            return result;
        }

        /// <summary>规范名 -> 部件类型 Id，用于 ConvertStandardInfosToConfigurations 回填 ComponentTypeId。</summary>
        private Dictionary<string, int> BuildComponentTypeNameToId()
        {
            var componentTypes = _context.S3dDictPipingComponentTypes.AsNoTracking().ToList();
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var ct in componentTypes)
            {
                var canonical = NormalizeComponentTypeNameToCanonical(ct.ComponentTypeName);
                if (!string.IsNullOrEmpty(canonical) && !result.ContainsKey(canonical))
                    result[canonical] = ct.Id;
            }
            return result;
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
        /// 解析PMC编码并返回基础信息和配置信息
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <returns>包含基础信息和配置信息的DTO</returns>
        public PmcInfoWithConfigDto AnalyzeCodeFromPMCWithConfig(string pmcCode)
        {
            // 先获取基础信息
            var baseInfo = AnalyzeCodeFromPMC(pmcCode);

            // 查询配置信息与规格书配置状态
            var configurations = new List<ComponentTypeConfiguration>();
            var configStatus = SpecConfigStatus.Pending;
            var entity = _context.S3dRulePmcdata
                .AsNoTracking()
                .FirstOrDefault(x => x.Pmccode == pmcCode);
            if (entity != null)
            {
                configStatus = SpecConfigStatus.Normalize(entity.Status);
                if (GetSpecRules(pmcCode, out var standardInfos) && standardInfos.Any())
                {
                    configurations = ConvertStandardInfosToConfigurations(standardInfos);
                }
            }

            return new PmcInfoWithConfigDto
            {
                BaseInfo = baseInfo,
                Configurations = configurations,
                ConfigStatus = configStatus
            };
        }

        /// <summary>
        /// 将标准信息列表转换为部件类型配置列表（简化版：仅保留标准名称和材料信息，去除通径范围）
        /// </summary>
        /// <param name="standardInfos">标准信息列表</param>
        /// <returns>部件类型配置列表</returns>
        private List<ComponentTypeConfiguration> ConvertStandardInfosToConfigurations(List<PmcStandardInfo> standardInfos)
        {
            var configurations = new Dictionary<string, ComponentTypeConfiguration>();
            var nameToId = BuildComponentTypeNameToId();

            foreach (var standardInfo in standardInfos)
            {
                // 跳过无效的标准信息
                if (string.IsNullOrWhiteSpace(standardInfo.StandardType) || string.IsNullOrWhiteSpace(standardInfo.StandardName))
                {
                    continue;
                }

                // 跳过重复范围默认配置（IsDefault == true），因为涉及通径范围
                if (standardInfo.IsDefault == true)
                {
                    continue;
                }

                // 获取或创建部件类型配置
                if (!configurations.ContainsKey(standardInfo.StandardType))
                {
                    var canonical = NormalizeComponentTypeNameToCanonical(standardInfo.StandardType);
                    var componentTypeId = standardInfo.ComponentTypeId ?? (canonical != null && nameToId.TryGetValue(canonical, out var id) ? id : (int?)null);
                    configurations[standardInfo.StandardType] = new ComponentTypeConfiguration
                    {
                        ComponentTypeId = componentTypeId,
                        ComponentType = standardInfo.StandardType,
                        FullConfig = new ComponentFullConfiguration
                        {
                            StandardFileConfigs = new List<StandardFileConfig>()
                        }
                    };
                }

                var config = configurations[standardInfo.StandardType];

                // 确保StandardFileConfigs列表已初始化
                if (config.FullConfig!.StandardFileConfigs == null)
                {
                    config.FullConfig.StandardFileConfigs = new List<StandardFileConfig>();
                }

                // 创建标准文件配置（仅包含标准名称和材料，不包含通径范围）
                var stdConfig = new StandardFileConfig
                {
                    StandardFile = standardInfo.StandardName,
                    Material = standardInfo.Material
                    // 注意：不包含 MinNpdValue、MaxNpdValue 和 BendRadiusMultiple，因为已简化配置
                };

                // 检查是否已存在相同的标准+材料组合，避免重复添加
                var exists = config.FullConfig.StandardFileConfigs.Any(x =>
                    x.StandardFile?.ToString() == stdConfig.StandardFile?.ToString() &&
                    x.Material?.ToString() == stdConfig.Material?.ToString());

                if (!exists)
                {
                    config.FullConfig.StandardFileConfigs.Add(stdConfig);
                }
            }

            return configurations.Values.ToList();
        }

        /// <summary>
        /// 生成对应的管系规格书
        /// </summary>
        /// <returns></returns>
        public bool GeneratePipeSpecTable()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool SetSpecConfigStatus(string pmcCode, string status, string? shipType = null, string? shipNumber = null)
        {
            if (string.IsNullOrWhiteSpace(pmcCode))
            {
                _logger.LogError("SetSpecConfigStatus: PmcCode 不能为空");
                throw new ArgumentException("PMC编码不能为空", nameof(pmcCode));
            }

            var query = _context.S3dRulePmcdata.Where(x => x.Pmccode == pmcCode);
            if (!string.IsNullOrWhiteSpace(shipType) && !string.IsNullOrWhiteSpace(shipNumber))
            {
                query = query.Where(x => x.ShipType == shipType && x.ShipNo == shipNumber);
            }

            var entity = query.FirstOrDefault();
            if (entity == null)
            {
                _logger.LogWarning("SetSpecConfigStatus: 未找到 PMC 编码 {PmcCode} 对应的记录", pmcCode);
                return false;
            }

            entity.Status = status;
            _context.SaveChanges();
            _logger.LogInformation("SetSpecConfigStatus: 已将 PMC {PmcCode} 规格书配置状态更新为 {Status}", pmcCode, status);
            return true;
        }

        /// <inheritdoc />
        public bool AcceptSpecReview(string pmcCode, string? shipType = null, string? shipNumber = null)
        {
            // TODO: 后续接入审核系统流程，目前占位默认审核成功
            return SetSpecConfigStatus(pmcCode, SpecConfigStatus.Approved, shipType, shipNumber);
        }
    }
}