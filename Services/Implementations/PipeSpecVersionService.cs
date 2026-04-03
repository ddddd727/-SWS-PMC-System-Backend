using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Models;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared.Constants;

namespace PMCSystem_Backend.Services.Implementations;

/// <summary>
/// 管系规格书版本管理服务
/// </summary>
public class PipeSpecVersionService : IPipeSpecVersionService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PipeSpecVersionService> _logger;

    /// <summary>版本 Standard 列 getter，用于收集所有标准配置</summary>
    private static readonly Func<PipeSpecVersion, List<PmcStandardInfo>?>[] StandardGetters =
    {
        v => v.PipeStandard,
        v => v.ElbowStandard,
        v => v.RedStandard,
        v => v.TeeStandard,
        v => v.SleeveStandard,
        v => v.BossesStandard,
        v => v.SaddlesStandard,
        v => v.CapsStandard,
        v => v.OverpassStandard,
        v => v.AccessoriesStandard,
        v => v.FlangeStandard,
        v => v.BlindFlangeStandard,
        v => v.GasketStandard,
        v => v.BoltStandard,
        v => v.NutStandard,
        v => v.WasherStandard
    };

    public PipeSpecVersionService(AppDbContext context, ILogger<PipeSpecVersionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public (List<PipeSpecVersionDto> Items, int TotalCount) GetVersionList(string pmcCode, string? shipType, string? shipNumber, int pageIndex = 1, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(pmcCode))
        {
            _logger.LogError("GetVersionList: PmcCode 不能为空");
            throw new ArgumentException("PMC 编码不能为空", nameof(pmcCode));
        }

        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.PipeSpecVersions
            .AsNoTracking()
            .Where(x => x.PmcCode == pmcCode);

        if (!string.IsNullOrWhiteSpace(shipType) && !string.IsNullOrWhiteSpace(shipNumber))
        {
            query = query.Where(x => x.ShipType == shipType && x.ShipNo == shipNumber);
        }

        var totalCount = query.Count();
        var items = query
            .OrderByDescending(x => x.Version)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new PipeSpecVersionDto
            {
                Id = x.Id,
                PmcCode = x.PmcCode,
                ShipType = x.ShipType,
                ShipNo = x.ShipNo,
                Version = x.Version,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Comment = x.Comment
            })
            .ToList();

        _logger.LogInformation("获取 PMC {PmcCode} 版本列表，共 {Total} 条，返回第 {Page} 页", pmcCode, totalCount, pageIndex);
        return (items, totalCount);
    }

    /// <inheritdoc />
    public PipeSpecVersionDetailDto? GetVersionDetail(int versionId)
    {
        var version = _context.PipeSpecVersions
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == versionId);

        if (version == null)
        {
            _logger.LogWarning("GetVersionDetail: 未找到版本 Id {VersionId}", versionId);
            return null;
        }

        var standardInfos = CollectStandardsFromVersion(version);
        var configurations = ConvertStandardInfosToConfigurations(standardInfos);

        var baseInfo = new PmcBaseInfoDto
        {
            PmcCode = version.PmcCode,
            ShipNumber = version.ShipNo,
            Status = version.Status ?? string.Empty,
            PipingClass = version.PipingClassName,
            MaterialGrade = version.MaterialsGradeName,
            PressureRating = version.PressureRatingName,
            PipeStandard = version.PipingStandardName,
            MaterialCategory = version.MaterialsCategoryName,
            WallThickness = version.ScheduleThicknessName
        };

        return new PipeSpecVersionDetailDto
        {
            Id = version.Id,
            Version = version.Version,
            CreatedAt = version.CreatedAt,
            CreatedBy = version.CreatedBy,
            Comment = version.Comment,
            BaseInfo = baseInfo,
            Configurations = configurations,
            ConfigStatus = SpecConfigStatus.Normalize(version.Status)
        };
    }

    /// <inheritdoc />
    public bool RevertToVersion(int versionId, string? shipType = null, string? shipNumber = null)
    {
        var version = _context.PipeSpecVersions
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == versionId);

        if (version == null)
        {
            _logger.LogWarning("RevertToVersion: 未找到版本 Id {VersionId}", versionId);
            return false;
        }

        var query = _context.S3dRulePmcdata.Where(x =>
            x.Pmccode == version.PmcCode &&
            x.ShipType == version.ShipType &&
            x.ShipNo == version.ShipNo);

        if (!string.IsNullOrWhiteSpace(shipType) && !string.IsNullOrWhiteSpace(shipNumber))
        {
            query = query.Where(x => x.ShipType == shipType && x.ShipNo == shipNumber);
        }

        var mainEntity = query.FirstOrDefault();
        if (mainEntity == null)
        {
            _logger.LogWarning("RevertToVersion: 未找到主表记录 PmcCode={PmcCode} ShipType={ShipType} ShipNo={ShipNo}",
                version.PmcCode, version.ShipType, version.ShipNo);
            return false;
        }

        CopyVersionToMain(version, mainEntity);
        mainEntity.Status = SpecConfigStatus.Review;
        _context.SaveChanges();

        _logger.LogInformation("已将 PMC {PmcCode} 回滚至版本 {Version}", version.PmcCode, version.Version);
        return true;
    }

    /// <inheritdoc />
    public void CreateVersionSnapshot(S3dRulePmcData currentEntity, string? createdBy = null, string? comment = null)
    {
        var nextVersion = _context.PipeSpecVersions
            .Where(x => x.PmcCode == currentEntity.Pmccode && x.ShipType == currentEntity.ShipType && x.ShipNo == currentEntity.ShipNo)
            .Max(x => (int?)x.Version) ?? 0;

        var hasConfig = currentEntity.PipeStandard?.Any() == true ||
                        currentEntity.ElbowStandard?.Any() == true ||
                        currentEntity.RedStandard?.Any() == true ||
                        currentEntity.TeeStandard?.Any() == true ||
                        currentEntity.SleeveStandard?.Any() == true ||
                        currentEntity.BossesStandard?.Any() == true ||
                        currentEntity.SaddlesStandard?.Any() == true ||
                        currentEntity.CapsStandard?.Any() == true ||
                        currentEntity.OverpassStandard?.Any() == true ||
                        currentEntity.AccessoriesStandard?.Any() == true ||
                        currentEntity.FlangeStandard?.Any() == true ||
                        currentEntity.BlindFlangeStandard?.Any() == true ||
                        currentEntity.GasketStandard?.Any() == true ||
                        currentEntity.BoltStandard?.Any() == true ||
                        currentEntity.NutStandard?.Any() == true ||
                        currentEntity.WasherStandard?.Any() == true;

        if (!hasConfig)
        {
            _logger.LogDebug("CreateVersionSnapshot: PMC {PmcCode} 当前无配置，跳过快照", currentEntity.Pmccode);
            return;
        }

        var snapshot = new PipeSpecVersion
        {
            PmcCode = currentEntity.Pmccode,
            ShipType = currentEntity.ShipType,
            ShipNo = currentEntity.ShipNo,
            Version = nextVersion + 1,
            PipingClassName = currentEntity.PipingClassName,
            MaterialsCategoryName = currentEntity.MaterialsCategoryName,
            PipingStandardName = currentEntity.PipingStandardName,
            MaterialsGradeName = currentEntity.MaterialsGradeName,
            FlangeStandardName = currentEntity.FlangeStandardName,
            PressureRatingName = currentEntity.PressureRatingName,
            ScheduleThicknessName = currentEntity.ScheduleThicknessName,
            PipeStandard = CloneList(currentEntity.PipeStandard),
            ElbowStandard = CloneList(currentEntity.ElbowStandard),
            RedStandard = CloneList(currentEntity.RedStandard),
            TeeStandard = CloneList(currentEntity.TeeStandard),
            SleeveStandard = CloneList(currentEntity.SleeveStandard),
            BossesStandard = CloneList(currentEntity.BossesStandard),
            SaddlesStandard = CloneList(currentEntity.SaddlesStandard),
            CapsStandard = CloneList(currentEntity.CapsStandard),
            OverpassStandard = CloneList(currentEntity.OverpassStandard),
            AccessoriesStandard = CloneList(currentEntity.AccessoriesStandard),
            FlangeStandard = CloneList(currentEntity.FlangeStandard),
            BlindFlangeStandard = CloneList(currentEntity.BlindFlangeStandard),
            GasketStandard = CloneList(currentEntity.GasketStandard),
            BoltStandard = CloneList(currentEntity.BoltStandard),
            NutStandard = CloneList(currentEntity.NutStandard),
            WasherStandard = CloneList(currentEntity.WasherStandard),
            Status = currentEntity.Status,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            Comment = comment
        };

        _context.PipeSpecVersions.Add(snapshot);
        _logger.LogInformation("已创建 PMC {PmcCode} 版本 {Version} 快照", currentEntity.Pmccode, snapshot.Version);
    }

    /// <summary>
    /// 从版本实体收集所有标准配置
    /// </summary>
    private static List<PmcStandardInfo> CollectStandardsFromVersion(PipeSpecVersion version)
    {
        var result = new List<PmcStandardInfo>();
        foreach (var getter in StandardGetters)
        {
            var list = getter(version);
            if (list != null && list.Count > 0)
            {
                result.AddRange(list);
            }
        }
        return result;
    }

    /// <summary>
    /// 将标准信息列表转换为部件类型配置列表
    /// </summary>
    private List<ComponentTypeConfiguration> ConvertStandardInfosToConfigurations(List<PmcStandardInfo> standardInfos)
    {
        var configurations = new Dictionary<string, ComponentTypeConfiguration>(StringComparer.OrdinalIgnoreCase);
        var nameToId = BuildComponentTypeNameToId();

        foreach (var standardInfo in standardInfos)
        {
            if (string.IsNullOrWhiteSpace(standardInfo.StandardType) || string.IsNullOrWhiteSpace(standardInfo.StandardName))
                continue;
            if (standardInfo.IsDefault == true)
                continue;

            var canonical = NormalizeComponentTypeNameToCanonical(standardInfo.StandardType);
            var key = canonical ?? standardInfo.StandardType;

            if (!configurations.ContainsKey(key))
            {
                var componentTypeId = standardInfo.ComponentTypeId ?? (canonical != null && nameToId.TryGetValue(canonical, out var id) ? id : (int?)null);
                configurations[key] = new ComponentTypeConfiguration
                {
                    ComponentTypeId = componentTypeId,
                    ComponentType = standardInfo.StandardType,
                    FullConfig = new ComponentFullConfiguration
                    {
                        StandardFileConfigs = new List<StandardFileConfig>()
                    }
                };
            }

            var config = configurations[key];
            if (config.FullConfig!.StandardFileConfigs == null)
                config.FullConfig.StandardFileConfigs = new List<StandardFileConfig>();

            var stdConfig = new StandardFileConfig
            {
                StandardFile = standardInfo.StandardName,
                Material = standardInfo.Material
            };

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
    /// 构建规范名 -> 部件类型 Id 映射
    /// </summary>
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
    /// 规范化为与 ComponentType 键一致的名（不区分大小写）
    /// </summary>
    private static string? NormalizeComponentTypeNameToCanonical(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var t = name.Trim();
        if (t.Length == 0) return null;
        var lower = t.ToLowerInvariant();
        var canonicalKeys = new[] { "elbow", "reducer", "tee", "sleeve", "bosses", "saddles", "caps", "overpass", "accessories", "flange", "blindflange", "gasket", "bolt", "nut", "washer", "pipe" };
        if (canonicalKeys.Contains(lower)) return lower;
        if (lower.EndsWith('s') && t.Length > 1 && canonicalKeys.Contains(lower[..^1]))
            return lower[..^1];
        if (lower.Replace(" ", "") == "blindflange" || lower.Contains("blind flange"))
            return "blindflange";
        return lower;
    }

    /// <summary>
    /// 深拷贝列表（浅拷贝每个元素，因 PmcStandardInfo 通常不再被修改）
    /// </summary>
    private static List<PmcStandardInfo>? CloneList(List<PmcStandardInfo>? source)
    {
        if (source == null || source.Count == 0)
            return null;
        return source.ToList();
    }

    /// <summary>
    /// 将版本快照数据复制到主表实体
    /// </summary>
    private static void CopyVersionToMain(PipeSpecVersion version, S3dRulePmcData mainEntity)
    {
        mainEntity.PipingClassName = version.PipingClassName ?? mainEntity.PipingClassName;
        mainEntity.MaterialsCategoryName = version.MaterialsCategoryName ?? mainEntity.MaterialsCategoryName;
        mainEntity.PipingStandardName = version.PipingStandardName ?? mainEntity.PipingStandardName;
        mainEntity.MaterialsGradeName = version.MaterialsGradeName ?? mainEntity.MaterialsGradeName;
        mainEntity.FlangeStandardName = version.FlangeStandardName ?? mainEntity.FlangeStandardName;
        mainEntity.PressureRatingName = version.PressureRatingName ?? mainEntity.PressureRatingName;
        mainEntity.ScheduleThicknessName = version.ScheduleThicknessName ?? mainEntity.ScheduleThicknessName;
        mainEntity.PipeStandard = version.PipeStandard;
        mainEntity.ElbowStandard = version.ElbowStandard;
        mainEntity.RedStandard = version.RedStandard;
        mainEntity.TeeStandard = version.TeeStandard;
        mainEntity.SleeveStandard = version.SleeveStandard;
        mainEntity.BossesStandard = version.BossesStandard;
        mainEntity.SaddlesStandard = version.SaddlesStandard;
        mainEntity.CapsStandard = version.CapsStandard;
        mainEntity.OverpassStandard = version.OverpassStandard;
        mainEntity.AccessoriesStandard = version.AccessoriesStandard;
        mainEntity.FlangeStandard = version.FlangeStandard;
        mainEntity.BlindFlangeStandard = version.BlindFlangeStandard;
        mainEntity.GasketStandard = version.GasketStandard;
        mainEntity.BoltStandard = version.BoltStandard;
        mainEntity.NutStandard = version.NutStandard;
        mainEntity.WasherStandard = version.WasherStandard;
    }
}
