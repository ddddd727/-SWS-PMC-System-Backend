using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dDictWallThicknessService : IS3dDictWallThicknessService
    {
        private readonly PmcContext _context;
        private readonly IMapper _mapper;

        public S3dDictWallThicknessService(PmcContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dDictWallThicknessDto>> GetAllListAsync()
        {
            var items = await _context.S3dDictWallThicknesses.ToListAsync();
            return _mapper.Map<List<S3dDictWallThicknessDto>>(items);
        }

        public async Task<S3dDictWallThicknessDto> CreateAsync(CreateS3dDictWallThicknessDto dto)
        {
            var entity = _mapper.Map<S3dDictWallThickness>(dto);
            entity.Status = dto.Status ?? true;
            
            // Manual ID calculation
            var maxId = await _context.S3dDictWallThicknesses.MaxAsync(e => (int?)e.Id) ?? 0;
            entity.Id = maxId + 1;
            
            _context.S3dDictWallThicknesses.Add(entity);

            await _context.Database.OpenConnectionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Dict_WallThickness ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Dict_WallThickness OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return _mapper.Map<S3dDictWallThicknessDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateS3dDictWallThicknessDto dto)
        {
            var entity = await _context.S3dDictWallThicknesses.FindAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            if (dto.Status.HasValue)
            {
                entity.Status = dto.Status.Value;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.S3dDictWallThicknesses.FindAsync(id);
            if (entity == null) return false;

            _context.S3dDictWallThicknesses.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
