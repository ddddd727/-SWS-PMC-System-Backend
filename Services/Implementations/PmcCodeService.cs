using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Services.Impletation
{
    public class PmcCodeService : IPmcCodeService
    {
        private readonly PmcTestContext _context;
        private readonly ILogger<PmcCodeService> _logger;

        public PmcCodeService(PmcTestContext context, ILogger<PmcCodeService> logger)
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

                var pipingClasses = _context.VwPipingClassWithCodes
                    .AsNoTracking()
                    .Where(x => aCodes.Contains(x.PipingClassCode))
                    .ToList();
                _logger.LogDebug("Fetched {Count} PipingClasses (A)", pipingClasses.Count);

                var materialsCategoryStandards = _context.VwMaterialsCategoryPipingStandards
                    .AsNoTracking()
                    .Where(x => b1Codes.Contains(x.MaterialsCategoryCode) || (x.PipingStandardCode != null && b2Codes.Contains(x.PipingStandardCode)))
                    .ToList();
                _logger.LogDebug("Fetched {Count} MaterialsCategoryStandards (B1/B2)", materialsCategoryStandards.Count);

                var pipingStandardGrades = _context.VwPipingStandardMaterialsGrades
                    .AsNoTracking()
                    .Where(x => (x.PipingStandardCode != null && b2Codes.Contains(x.PipingStandardCode)) || b3Codes.Contains(x.MaterialsGradeCode))
                    .ToList();
                _logger.LogDebug("Fetched {Count} PipingStandardGrades (B2/B3)", pipingStandardGrades.Count);

                var flangeStandRatings = _context.VwFlangeStandPressureRatings
                    .AsNoTracking()
                    .Where(x => c1Codes.Contains(x.FlangeStandardCode) || (x.PressureRatingCode != null && c2Codes.Contains(x.PressureRatingCode)))
                    .ToList();
                _logger.LogDebug("Fetched {Count} FlangeStandRatings (C1/C2)", flangeStandRatings.Count);

                var pipingStandardThicknesses = _context.VwPipingStandardScheduleThicknesses
                    .AsNoTracking()
                    .Where(x => (x.PipingStandardCode != null && b2Codes.Contains(x.PipingStandardCode)) || dCodes.Contains(x.ScheduleThicknessCode))
                    .ToList();
                _logger.LogDebug("Fetched {Count} PipingStandardThicknesses (B2/D)", pipingStandardThicknesses.Count);

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

                    var dEntity = pipingStandardThicknesses
                        .FirstOrDefault(x => x.PipingStandardCode == b2 && x.ScheduleThicknessCode == d);
                    if (dEntity != null)
                    {
                        item.DDesc = dEntity.ScheduleThicknessDesc;
                    }
                    else
                    {
                        _logger.LogWarning("PMC {Pmc}: D segment '{D}' (with B2='{B2}') not found in PipingStandardThicknesses", pmc, d, b2);
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
    }
}

