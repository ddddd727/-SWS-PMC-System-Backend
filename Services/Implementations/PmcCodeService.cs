using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMCSystem_Backend.Services.Interface;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Services.Impletation
{
    public class PmcCodeService : IPmcCodeService
    {
        private readonly PmcContextLr _context;
        private readonly ILogger<PmcCodeService> _logger;

        public PmcCodeService(PmcContextLr context, ILogger<PmcCodeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<PmcCodeGenerateResponseItem> GenerateWithDescriptions(IEnumerable<string> pmcCodes)
        {
            try
            {
                var result = new List<PmcCodeGenerateResponseItem>();

                var distinctCodes = pmcCodes
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 7)
                    .Distinct()
                    .ToList();

                if (distinctCodes.Count == 0)
                {
                    return result;
                }

                _logger.LogInformation("Generating descriptions for {Count} PMC codes: {Codes}", distinctCodes.Count, string.Join(", ", distinctCodes));

                var aCodes = distinctCodes.Select(x => x.Substring(0, 1)).Distinct().ToList();
                var b1Codes = distinctCodes.Select(x => x.Substring(1, 1)).Distinct().ToList();
                var b2Codes = distinctCodes.Select(x => x.Substring(2, 1)).Distinct().ToList();
                var b3Codes = distinctCodes.Select(x => x.Substring(3, 1)).Distinct().ToList();
                var c1Codes = distinctCodes.Select(x => x.Substring(4, 1)).Distinct().ToList();
                var c2Codes = distinctCodes.Select(x => x.Substring(5, 1)).Distinct().ToList();
                var dCodes = distinctCodes.Select(x => x.Substring(6, 1)).Distinct().ToList();

                _logger.LogDebug("Distinct segments - A: {A}, B1: {B1}, B2: {B2}, B3: {B3}, C1: {C1}, C2: {C2}, D: {D}", 
                    string.Join(",", aCodes), string.Join(",", b1Codes), string.Join(",", b2Codes), 
                    string.Join(",", b3Codes), string.Join(",", c1Codes), string.Join(",", c2Codes), string.Join(",", dCodes));

                var pipingClasses = _context.S3dCodePipingClasses
                    .AsNoTracking()
                    .Where(x => aCodes.Contains(x.PipingClassCode))
                    .ToList();
                _logger.LogDebug("Fetched {Count} PipingClasses (A)", pipingClasses.Count);

                var materialsCategoryStandards = _context.S3dCodeMaterialsCategoryPipingStandards
                    .AsNoTracking()
                    .Where(x => b1Codes.Contains(x.MaterialsCategoryCode) || (x.PipingStandardCode != null && b2Codes.Contains(x.PipingStandardCode)))
                    .ToList();
                _logger.LogDebug("Fetched {Count} MaterialsCategoryStandards (B1/B2)", materialsCategoryStandards.Count);

                var pipingStandardGrades = _context.S3dCodePipingStandardMaterialsGrades
                    .AsNoTracking()
                    .Where(x => (x.PipingStandardCode != null && b2Codes.Contains(x.PipingStandardCode)) || b3Codes.Contains(x.MaterialsGradeCode))
                    .ToList();
                _logger.LogDebug("Fetched {Count} PipingStandardGrades (B2/B3)", pipingStandardGrades.Count);

                var flangeStandRatings = _context.S3dCodeFlangeStandPressureRatings
                    .AsNoTracking()
                    .Where(x => c1Codes.Contains(x.FlangeStandardCode) || (x.PressureRatingCode != null && c2Codes.Contains(x.PressureRatingCode)))
                    .ToList();
                _logger.LogDebug("Fetched {Count} FlangeStandRatings (C1/C2)", flangeStandRatings.Count);

                var scheduleThicknesses = _context.S3dCodeMaterialsCategoryScheduleThicknesses
                    .AsNoTracking()
                    .Where(x => b1Codes.Contains(x.MaterialsCategoryCode) && dCodes.Contains(x.ScheduleThicknessCode))
                    .ToList();
                _logger.LogDebug("Fetched {Count} MaterialsCategoryScheduleThicknesses (B1/D)", scheduleThicknesses.Count);

                foreach (var pmc in distinctCodes)
                {
                    var a = pmc.Substring(0, 1);
                    var b1 = pmc.Substring(1, 1);
                    var b2 = pmc.Substring(2, 1);
                    var b3 = pmc.Substring(3, 1);
                    var c1 = pmc.Substring(4, 1);
                    var c2 = pmc.Substring(5, 1);
                    var d = pmc.Substring(6, 1);

                    var item = new PmcCodeGenerateResponseItem
                    {
                        Pmc = pmc,
                        A = a,
                        B1 = b1,
                        B2 = b2,
                        B3 = b3,
                        C1 = c1,
                        C2 = c2,
                        D = d
                    };

                    var aEntity = pipingClasses.FirstOrDefault(x => x.PipingClassCode == a);
                    if (aEntity != null)
                    {
                        item.ADesc = aEntity.ShortStringValue;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: A segment '{A}' not found in PipingClasses", pmc, a);
                    }

                    var b1Entity = materialsCategoryStandards
                        .FirstOrDefault(x => x.MaterialsCategoryCode == b1);
                    if (b1Entity != null)
                    {
                        item.B1Desc = b1Entity.MaterialsCategoryDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: B1 segment '{B1}' not found in MaterialsCategoryStandards", pmc, b1);
                    }

                    var b2Entity = materialsCategoryStandards
                        .FirstOrDefault(x => x.MaterialsCategoryCode == b1 && x.PipingStandardCode == b2);
                    if (b2Entity != null)
                    {
                        item.B2Desc = b2Entity.PipeStandDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: B2 segment '{B2}' (with B1='{B1}') not found in MaterialsCategoryStandards", pmc, b2, b1);
                    }

                    var b3Entity = pipingStandardGrades
                        .FirstOrDefault(x => x.PipingStandardCode == b2 && x.MaterialsGradeCode == b3);
                    if (b3Entity != null)
                    {
                        item.B3Desc = b3Entity.MaterialsGradeDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: B3 segment '{B3}' (with B2='{B2}') not found in PipingStandardGrades", pmc, b3, b2);
                    }

                    var c1Entity = flangeStandRatings
                        .FirstOrDefault(x => x.FlangeStandardCode == c1);
                    if (c1Entity != null)
                    {
                        item.C1Desc = c1Entity.FlangeStandDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: C1 segment '{C1}' not found in FlangeStandRatings", pmc, c1);
                    }

                    var c2Entity = flangeStandRatings
                        .FirstOrDefault(x => x.FlangeStandardCode == c1 && x.PressureRatingCode == c2);
                    if (c2Entity != null)
                    {
                        item.C2Desc = c2Entity.PressureRatingDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: C2 segment '{C2}' (with C1='{C1}') not found in FlangeStandRatings", pmc, c2, c1);
                    }

                    var dEntity = scheduleThicknesses
                        .FirstOrDefault(x => x.MaterialsCategoryCode == b1 && x.ScheduleThicknessCode == d);
                    if (dEntity != null)
                    {
                        item.DDesc = dEntity.ScheduleThicknessDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: D segment '{D}' (with B1='{B1}') not found in MaterialsCategoryScheduleThicknesses", pmc, d, b1);
                    }

                    result.Add(item);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateWithDescriptions for codes: {Codes}", string.Join(",", pmcCodes));
                throw;
            }
        }

        public void SavePmcCodes(PmcCodeSaveRequest request)
        {
            try
            {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var shipType = (request.ShipType ?? string.Empty).Trim();
                var shipNo = (request.ShipNo ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(shipType) || string.IsNullOrWhiteSpace(shipNo))
                    throw new ArgumentException("ShipType or ShipNo is empty.");

                var items = (request.Items ?? new List<PmcCodeSaveItem>())
                    .Where(x => !string.IsNullOrWhiteSpace(x.PmcCode))
                    .ToList();

                // 1. Fetch existing for this ShipType + ShipNo
                var existing = _context.S3dRulePmcdata
                    .Where(x => x.ShipType == shipType && x.ShipNo == shipNo)
                    .ToList();
                var existingByCode = existing.ToDictionary(x => x.Pmccode, x => x);

                // 2. Determine Add / Update / Delete
                // Modified: Changed to Append-Only mode as per requirement.
                // - If PMC exists: Do nothing (keep existing)
                // - If PMC not exists: Add new
                // - Do NOT delete any existing records not in the list

                // Upsert
                foreach (var item in items)
                {
                    if (!existingByCode.TryGetValue(item.PmcCode, out var entity))
                    {
                        // Add only if not exists
                        entity = new S3dRulePmcdatum
                        {
                            ShipType = shipType,
                            ShipNo = shipNo,
                            Pmccode = item.PmcCode,
                            PipingClassName = item.PipingClassName,
                            MaterialsCategoryName = item.MaterialsCategoryName,
                            PipingStandardName = item.PipingStandardName,
                            MaterialsGradeName = item.MaterialsGradeName,
                            FlangeStandardName = item.FlangeStandardName,
                            PressureRatingName = item.PressureRatingName,
                            ScheduleThicknessName = item.ScheduleThicknessName,
                            IsByRule = item.IsByRule
                        };
                        _context.S3dRulePmcdata.Add(entity);
                        // Update dict to avoid duplicates if input has dupes
                        existingByCode[item.PmcCode] = entity;
                    }
                    // Else: Exists -> Do nothing
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SavePmcCodes for ShipType={ShipType}, ShipNo={ShipNo}", request?.ShipType, request?.ShipNo);
                throw;
            }
        }

        public int DeletePmcCodes(PmcCodeDeleteRequest request)
        {
            try
            {
                if (request == null) throw new ArgumentNullException(nameof(request));
                var shipType = (request.ShipType ?? string.Empty).Trim();
                var shipNo = (request.ShipNo ?? string.Empty).Trim();
                var pmcCodes = request.PmcCodes ?? new List<string>();

                if (string.IsNullOrWhiteSpace(shipType) || string.IsNullOrWhiteSpace(shipNo))
                    throw new ArgumentException("ShipType or ShipNo is empty.");

                var distinctCodes = pmcCodes.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

                if (distinctCodes.Count == 0) return 0;

                var toDelete = _context.S3dRulePmcdata
                    .Where(x => x.ShipType == shipType && x.ShipNo == shipNo && distinctCodes.Contains(x.Pmccode))
                    .ToList();

                if (toDelete.Count > 0)
                {
                    _context.S3dRulePmcdata.RemoveRange(toDelete);
                    _context.SaveChanges();
                }

                return toDelete.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeletePmcCodes for ShipType={ShipType}, ShipNo={ShipNo}", request?.ShipType, request?.ShipNo);
                throw;
            }
        }

        public IEnumerable<PmcCodeQueryItem> GetPmcCodes(string shipType, string shipNo)
        {
            return _context.S3dRulePmcdata
                .Where(x => x.ShipType == shipType && x.ShipNo == shipNo)
                .Select(x => new PmcCodeQueryItem
                {
                    PmcCode = x.Pmccode,
                    PipingClassName = x.PipingClassName,
                    MaterialsCategoryName = x.MaterialsCategoryName,
                    PipingStandardName = x.PipingStandardName,
                    MaterialsGradeName = x.MaterialsGradeName,
                    FlangeStandardName = x.FlangeStandardName,
                    PressureRatingName = x.PressureRatingName,
                    ScheduleThicknessName = x.ScheduleThicknessName,
                    IsByRule = x.IsByRule
                })
                .ToList();
        }

        public IEnumerable<PmcOptionDto> GetOptions(string type, string? parentDesc = null)
        {
            switch (type.ToLower())
            {
                case "a": // 管材等级
                    return _context.S3dCodePipingClasses
                        .Select(x => new { Desc = x.ShortStringValue, Code = x.PipingClassCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                case "b1": // 主材料
                    return _context.S3dCodeMaterialsCategoryPipingStandards
                        .Select(x => new { Desc = x.MaterialsCategoryDesc, Code = x.MaterialsCategoryCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                case "b2": // 管材标准 (级联: parentDesc = B1 Desc)
                    var qB2 = _context.S3dCodeMaterialsCategoryPipingStandards.AsQueryable();
                    if (!string.IsNullOrWhiteSpace(parentDesc))
                    {
                        qB2 = qB2.Where(x => x.MaterialsCategoryDesc == parentDesc);
                    }
                    return qB2
                        .Where(x => !string.IsNullOrEmpty(x.PipeStandDesc) && !string.IsNullOrEmpty(x.PipingStandardCode))
                        .Select(x => new { Desc = x.PipeStandDesc, Code = x.PipingStandardCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                case "b3": // 牌号
                    return _context.S3dCodePipingStandardMaterialsGrades
                        .Select(x => new { Desc = x.MaterialsGradeDesc, Code = x.MaterialsGradeCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                case "c1": // 法兰标准
                    return _context.S3dCodeFlangeStandPressureRatings
                        .Select(x => new { Desc = x.FlangeStandDesc, Code = x.FlangeStandardCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                case "c2": // 法兰压力等级
                    return _context.S3dCodeFlangeStandPressureRatings
                        .Where(x => !string.IsNullOrEmpty(x.PressureRatingDesc) && !string.IsNullOrEmpty(x.PressureRatingCode))
                        .Select(x => new { Desc = x.PressureRatingDesc, Code = x.PressureRatingCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                case "d": // 壁厚等级
                    return _context.S3dCodeMaterialsCategoryScheduleThicknesses
                        .Select(x => new { Desc = x.ScheduleThicknessDesc, Code = x.ScheduleThicknessCode })
                        .Distinct()
                        .Select(x => new PmcOptionDto { Label = x.Desc, Value = x.Code })
                        .ToList();

                default:
                    return new List<PmcOptionDto>();
            }
        }

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
                new ShipInfo { shipNumber = "H1603", shipType = "民船" }
            };
            return shipInfos;
        }

        public int CopyRules(CopyRuleRequest request)
        {
            try
            {
                if (request == null) throw new ArgumentNullException(nameof(request));
                
                var sourceShipType = (request.SourceShipType ?? string.Empty).Trim();
                var sourceShipNo = (request.SourceShipNo ?? string.Empty).Trim();
                var targetShipType = (request.TargetShipType ?? string.Empty).Trim();
                var targetShipNo = (request.TargetShipNo ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(sourceShipType) || string.IsNullOrEmpty(sourceShipNo) ||
                    string.IsNullOrEmpty(targetShipType) || string.IsNullOrEmpty(targetShipNo))
                {
                    throw new ArgumentException("源船型船号和目标船型船号都不能为空");
                }

                // 1. 查询源数据
                var sourceItems = _context.S3dRulePmcdata
                    .Where(x => x.ShipType == sourceShipType && x.ShipNo == sourceShipNo)
                    .AsNoTracking()
                    .ToList();

                if (sourceItems.Count == 0)
                {
                    throw new Exception($"未查到源船型船号 ({sourceShipType} - {sourceShipNo}) 的数据");
                }

                // 2. 删除目标船型船号的现有数据 (Overwrite Mode)
                var existingTargetItems = _context.S3dRulePmcdata
                    .Where(x => x.ShipType == targetShipType && x.ShipNo == targetShipNo)
                    .ToList();
                
                if (existingTargetItems.Count > 0)
                {
                    _context.S3dRulePmcdata.RemoveRange(existingTargetItems);
                }

                // 3. 准备新数据
                var newItems = sourceItems.Select(src => new S3dRulePmcdatum
                {
                    ShipType = targetShipType,
                    ShipNo = targetShipNo,
                    Pmccode = src.Pmccode,
                    PipingClassName = src.PipingClassName,
                    MaterialsCategoryName = src.MaterialsCategoryName,
                    PipingStandardName = src.PipingStandardName,
                    MaterialsGradeName = src.MaterialsGradeName,
                    FlangeStandardName = src.FlangeStandardName,
                    PressureRatingName = src.PressureRatingName,
                    ScheduleThicknessName = src.ScheduleThicknessName,
                    IsByRule = src.IsByRule
                }).ToList();

                // 4. 批量插入
                _context.S3dRulePmcdata.AddRange(newItems);
                _context.SaveChanges();

                _logger.LogInformation("Copied {Count} rules from {Source} to {Target}", newItems.Count, $"{sourceShipType}-{sourceShipNo}", $"{targetShipType}-{targetShipNo}");
                
                return newItems.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to copy rules");
                throw;
            }
        }
    }
}