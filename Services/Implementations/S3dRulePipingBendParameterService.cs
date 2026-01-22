using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dRulePipingBendParameterService : IS3dRulePipingBendParameterService
    {
        private readonly PmcContext _context;
        private readonly IMapper _mapper;

        public S3dRulePipingBendParameterService(PmcContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dRulePipingBendParameterDto>> GetAllListAsync()
        {
            var items = await _context.S3dRulePipingBendParameters.ToListAsync();
            return _mapper.Map<List<S3dRulePipingBendParameterDto>>(items);
        }

        public async Task<S3dRulePipingBendParameterDto> CreateAsync(CreateS3dRulePipingBendParameterDto dto)
        {
            var entity = _mapper.Map<S3dRulePipingBendParameter>(dto);
            entity.Status = dto.Status ?? true;

            var maxId = await _context.S3dRulePipingBendParameters.MaxAsync(e => (int?)e.Id) ?? 0;
            entity.Id = maxId + 1;

            _context.S3dRulePipingBendParameters.Add(entity);

            await _context.Database.OpenConnectionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Rule_PipingBendParameter ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Rule_PipingBendParameter OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return _mapper.Map<S3dRulePipingBendParameterDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateS3dRulePipingBendParameterDto dto)
        {
            var entity = await _context.S3dRulePipingBendParameters.FindAsync(dto.Id);
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
            var entity = await _context.S3dRulePipingBendParameters.FindAsync(id);
            if (entity == null) return false;

            _context.S3dRulePipingBendParameters.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

