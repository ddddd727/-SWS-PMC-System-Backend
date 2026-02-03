using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    /// <summary>
    /// S3dRulePmcData 服务实现
    /// </summary>
    public class S3dRulePmcDataService : IS3dRulePmcDataService
    {
        private readonly PmcContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<S3dRulePmcDataService> _logger;

        public S3dRulePmcDataService(PmcContext context, IMapper mapper, ILogger<S3dRulePmcDataService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 根据ID获取单条记录
        /// </summary>
        public async Task<S3dRulePmcDataDto?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.S3dRulePmcdata
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("未找到ID为 {Id} 的S3dRulePmcData记录", id);
                    return null;
                }

                return _mapper.Map<S3dRulePmcDataDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取S3dRulePmcData记录失败，ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<PagedResult<S3dRulePmcDataDto>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                var query = _context.S3dRulePmcdata.AsQueryable();

                // 关键词搜索
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    var keyword = request.Keyword.Trim();
                    query = query.Where(x =>
                        x.Pmccode.Contains(keyword) ||
                        x.ShipNo.Contains(keyword) ||
                        x.ShipType.Contains(keyword) ||
                        (x.PipingClassName != null && x.PipingClassName.Contains(keyword)) ||
                        (x.MaterialsCategoryName != null && x.MaterialsCategoryName.Contains(keyword))
                    );
                }

                // 排序
                query = request.SortBy?.ToLower() switch
                {
                    "pmccode" => request.IsDescending
                        ? query.OrderByDescending(x => x.Pmccode)
                        : query.OrderBy(x => x.Pmccode),
                    "shipno" => request.IsDescending
                        ? query.OrderByDescending(x => x.ShipNo)
                        : query.OrderBy(x => x.ShipNo),
                    "shiptype" => request.IsDescending
                        ? query.OrderByDescending(x => x.ShipType)
                        : query.OrderBy(x => x.ShipType),
                    "id" => request.IsDescending
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id),
                    _ => query.OrderByDescending(x => x.Id) // 默认按ID降序
                };

                // 获取总数
                var totalCount = await query.CountAsync();

                // 分页
                var items = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .AsNoTracking()
                    .ToListAsync();

                var dtos = _mapper.Map<List<S3dRulePmcDataDto>>(items);

                return new PagedResult<S3dRulePmcDataDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页查询S3dRulePmcData失败");
                throw;
            }
        }

        /// <summary>
        /// 根据PMC编码查询
        /// </summary>
        public async Task<List<S3dRulePmcDataDto>> GetByPmcCodeAsync(string pmcCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pmcCode))
                {
                    throw new ArgumentException("PMC编码不能为空", nameof(pmcCode));
                }

                var entities = await _context.S3dRulePmcdata
                    .AsNoTracking()
                    .Where(x => x.Pmccode == pmcCode)
                    .ToListAsync();

                return _mapper.Map<List<S3dRulePmcDataDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据PMC编码查询S3dRulePmcData失败，PMC编码: {PmcCode}", pmcCode);
                throw;
            }
        }

        /// <summary>
        /// 根据船号查询
        /// </summary>
        public async Task<List<S3dRulePmcDataDto>> GetByShipNoAsync(string shipNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(shipNo))
                {
                    throw new ArgumentException("船号不能为空", nameof(shipNo));
                }

                var entities = await _context.S3dRulePmcdata
                    .AsNoTracking()
                    .Where(x => x.ShipNo == shipNo)
                    .ToListAsync();

                return _mapper.Map<List<S3dRulePmcDataDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据船号查询S3dRulePmcData失败，船号: {ShipNo}", shipNo);
                throw;
            }
        }

        /// <summary>
        /// 创建新记录
        /// </summary>
        public async Task<S3dRulePmcDataDto> CreateAsync(CreateS3dRulePmcDataDto dto)
        {
            try
            {
                // 检查唯一性约束（PMC编码、船型、船号的组合）
                var exists = await _context.S3dRulePmcdata
                    .AnyAsync(x => x.Pmccode == dto.Pmccode &&
                                   x.ShipType == dto.ShipType &&
                                   x.ShipNo == dto.ShipNo);

                if (exists)
                {
                    throw new InvalidOperationException(
                        $"PMC编码 {dto.Pmccode}、船型 {dto.ShipType}、船号 {dto.ShipNo} 的组合已存在");
                }

                var entity = _mapper.Map<S3dRulePmcData>(dto);

                _context.S3dRulePmcdata.Add(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("成功创建S3dRulePmcData记录，ID: {Id}", entity.Id);

                return _mapper.Map<S3dRulePmcDataDto>(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "创建S3dRulePmcData记录时数据库更新失败");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建S3dRulePmcData记录失败");
                throw;
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        public async Task<bool> UpdateAsync(int id, UpdateS3dRulePmcDataDto dto)
        {
            try
            {
                var entity = await _context.S3dRulePmcdata
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("未找到ID为 {Id} 的S3dRulePmcData记录", id);
                    return false;
                }

                // 如果更新了PMC编码、船型或船号，需要检查唯一性约束
                if ((!string.IsNullOrWhiteSpace(dto.Pmccode) && dto.Pmccode != entity.Pmccode) ||
                    (!string.IsNullOrWhiteSpace(dto.ShipType) && dto.ShipType != entity.ShipType) ||
                    (!string.IsNullOrWhiteSpace(dto.ShipNo) && dto.ShipNo != entity.ShipNo))
                {
                    var newPmcCode = dto.Pmccode ?? entity.Pmccode;
                    var newShipType = dto.ShipType ?? entity.ShipType;
                    var newShipNo = dto.ShipNo ?? entity.ShipNo;

                    var exists = await _context.S3dRulePmcdata
                        .AnyAsync(x => x.Id != id &&
                                       x.Pmccode == newPmcCode &&
                                       x.ShipType == newShipType &&
                                       x.ShipNo == newShipNo);

                    if (exists)
                    {
                        throw new InvalidOperationException(
                            $"PMC编码 {newPmcCode}、船型 {newShipType}、船号 {newShipNo} 的组合已存在");
                    }
                }

                // 更新实体属性（只更新非空属性）
                if (!string.IsNullOrWhiteSpace(dto.ShipType))
                    entity.ShipType = dto.ShipType;
                if (!string.IsNullOrWhiteSpace(dto.ShipNo))
                    entity.ShipNo = dto.ShipNo;
                if (!string.IsNullOrWhiteSpace(dto.Pmccode))
                    entity.Pmccode = dto.Pmccode;
                if (!string.IsNullOrWhiteSpace(dto.PipingClassName))
                    entity.PipingClassName = dto.PipingClassName;
                if (!string.IsNullOrWhiteSpace(dto.MaterialsCategoryName))
                    entity.MaterialsCategoryName = dto.MaterialsCategoryName;

                entity.PipingStandardName = dto.PipingStandardName ?? entity.PipingStandardName;
                entity.MaterialsGradeName = dto.MaterialsGradeName ?? entity.MaterialsGradeName;
                entity.FlangeStandardName = dto.FlangeStandardName ?? entity.FlangeStandardName;
                entity.PressureRatingName = dto.PressureRatingName ?? entity.PressureRatingName;
                entity.ScheduleThicknessName = dto.ScheduleThicknessName ?? entity.ScheduleThicknessName;
                entity.PipeStandard = dto.PipeStandard ?? entity.PipeStandard;
                entity.ElbowStandard = dto.ElbowStandard ?? entity.ElbowStandard;
                entity.RedStandard = dto.RedStandard ?? entity.RedStandard;
                entity.TeeStandard = dto.TeeStandard ?? entity.TeeStandard;
                entity.SleeveStandard = dto.SleeveStandard ?? entity.SleeveStandard;
                entity.BossesStandard = dto.BossesStandard ?? entity.BossesStandard;
                entity.SaddlesStandard = dto.SaddlesStandard ?? entity.SaddlesStandard;
                entity.CapsStandard = dto.CapsStandard ?? entity.CapsStandard;
                entity.OverpassStandard = dto.OverpassStandard ?? entity.OverpassStandard;
                entity.AccessoriesStandard = dto.AccessoriesStandard ?? entity.AccessoriesStandard;
                entity.FlangeStandard = dto.FlangeStandard ?? entity.FlangeStandard;
                entity.BlindFlangeStandard = dto.BlindFlangeStandard ?? entity.BlindFlangeStandard;
                entity.GasketStandard = dto.GasketStandard ?? entity.GasketStandard;
                entity.BoltStandard = dto.BoltStandard ?? entity.BoltStandard;
                entity.NutStandard = dto.NutStandard ?? entity.NutStandard;
                entity.WasherStandard = dto.WasherStandard ?? entity.WasherStandard;
                entity.Status = dto.Status ?? entity.Status;
                entity.VersionNum = dto.VersionNum ?? entity.VersionNum;
                entity.IsByRule = dto.IsByRule ?? entity.IsByRule;
                entity.JsonData = dto.JsonData ?? entity.JsonData;

                await _context.SaveChangesAsync();

                _logger.LogInformation("成功更新S3dRulePmcData记录，ID: {Id}", id);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "更新S3dRulePmcData记录时数据库更新失败，ID: {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新S3dRulePmcData记录失败，ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.S3dRulePmcdata
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("未找到ID为 {Id} 的S3dRulePmcData记录", id);
                    return false;
                }

                _context.S3dRulePmcdata.Remove(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("成功删除S3dRulePmcData记录，ID: {Id}", id);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "删除S3dRulePmcData记录时数据库更新失败，ID: {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除S3dRulePmcData记录失败，ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        public async Task<int> DeleteBatchAsync(List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return 0;
                }

                var entities = await _context.S3dRulePmcdata
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();

                if (entities.Count == 0)
                {
                    return 0;
                }

                _context.S3dRulePmcdata.RemoveRange(entities);
                var deletedCount = await _context.SaveChangesAsync();

                _logger.LogInformation("成功批量删除S3dRulePmcData记录，删除数量: {Count}", deletedCount);
                return deletedCount;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "批量删除S3dRulePmcData记录时数据库更新失败");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量删除S3dRulePmcData记录失败");
                throw;
            }
        }
    }
}
