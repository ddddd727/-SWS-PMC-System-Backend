using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class DspSpmcDictPipingBendDataService : IDspSpmcDictPipingBendDataService
    {
        private readonly PmcTestContext _context;
        private readonly IMapper _mapper;

        public DspSpmcDictPipingBendDataService(PmcTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DspSpmcDictPipingBendDataDto>> GetAllListAsync()
        {
            var items = await _context.S3dDictPipingBendData.ToListAsync();
            return _mapper.Map<List<DspSpmcDictPipingBendDataDto>>(items);
        }

        public async Task<DspSpmcDictPipingBendDataDto> CreateAsync(CreateDspSpmcDictPipingBendDataDto dto)
        {
            var entity = _mapper.Map<S3dDictPipingBendDatum>(dto);
            entity.Status = dto.Status ?? true;
            var maxId = await _context.S3dDictPipingBendData.MaxAsync(e => (int?)e.Id) ?? 0;
            entity.Id = maxId + 1;
            _context.S3dDictPipingBendData.Add(entity);

            await _context.Database.OpenConnectionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Dict_PipingBendData ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Dict_PipingBendData OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return _mapper.Map<DspSpmcDictPipingBendDataDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateDspSpmcDictPipingBendDataDto dto)
        {
            var entity = await _context.S3dDictPipingBendData.FindAsync(dto.Id);
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
            var entity = await _context.S3dDictPipingBendData.FindAsync(id);
            if (entity == null) return false;

            _context.S3dDictPipingBendData.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
