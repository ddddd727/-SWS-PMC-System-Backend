using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Shared.Entities;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCommonPlainPipingGenericDataService : IS3dCommonPlainPipingGenericDataService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public S3dCommonPlainPipingGenericDataService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dCommonPlainPipingGenericDataDto>> GetAllListAsync()
        {
            var items = await _context.S3dCommonPlainPipingGenericData
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<S3dCommonPlainPipingGenericDataDto>>(items);
        }

        public async Task<S3dCommonPlainPipingGenericDataDto> CreateAsync(CreateS3dCommonPlainPipingGenericDataDto dto)
        {
            var entity = _mapper.Map<S3dCommonPlainPipingGenericData>(dto);

            var now = DateTime.Now;
            entity.Status = dto.Status ?? true;
            entity.CreatedDate = now;
            entity.ModifiedDate = now;

            _context.S3dCommonPlainPipingGenericData.Add(entity);
            await SaveChangesWithDuplicateCheckAsync();

            return _mapper.Map<S3dCommonPlainPipingGenericDataDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateS3dCommonPlainPipingGenericDataDto dto)
        {
            var entity = await _context.S3dCommonPlainPipingGenericData.FindAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            if (dto.Status.HasValue)
            {
                entity.Status = dto.Status.Value;
            }

            entity.ModifiedDate = DateTime.Now;

            await SaveChangesWithDuplicateCheckAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.S3dCommonPlainPipingGenericData.FindAsync(id);
            if (entity == null) return false;

            _context.S3dCommonPlainPipingGenericData.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task SaveChangesWithDuplicateCheckAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new InvalidOperationException("记录已存在");
            }
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is not SqlException sqlEx) return false;
            return sqlEx.Number == 2601 || sqlEx.Number == 2627;
        }
    }
}
