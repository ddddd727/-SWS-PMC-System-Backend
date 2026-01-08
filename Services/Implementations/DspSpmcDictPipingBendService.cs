using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class DspSpmcDictPipingBendService : IDspSpmcDictPipingBendService
    {
        private readonly PmcTestContext _context;
        private readonly IMapper _mapper;

        public DspSpmcDictPipingBendService(PmcTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DspSpmcDictPipingBendDto>> GetAllListAsync()
        {
            var items = await _context.DspSpmcDictPipingBends.ToListAsync();
            return _mapper.Map<List<DspSpmcDictPipingBendDto>>(items);
        }

        public async Task<DspSpmcDictPipingBendDto?> GetByIdAsync(int id)
        {
            var entity = await _context.DspSpmcDictPipingBends.FindAsync(id);
            return _mapper.Map<DspSpmcDictPipingBendDto>(entity);
        }

        public async Task<DspSpmcDictPipingBendDto> CreateAsync(CreateDspSpmcDictPipingBendDto dto)
        {
            var entity = _mapper.Map<DspSpmcDictPipingBend>(dto);
            
            // 手动计算下一个 ID
            var maxId = await _context.DspSpmcDictPipingBends.MaxAsync(e => (int?)e.Id) ?? 0;
            entity.Id = maxId + 1;
            
            _context.DspSpmcDictPipingBends.Add(entity);
            
            // 如果数据库开启了 IDENTITY_INSERT，可能需要显式处理，
            // 但如果之前是自动增长，EF Core 通常会忽略手动设置的 ID，除非配置了 ValueGeneratedNever。
            // 这里我们假设用户想要填补空缺或保持连续性，
            // 但标准做法是 IDENTITY 列不可手动插入。
            // 为了强行插入 ID，我们需要开启 IDENTITY_INSERT。
            
            await _context.Database.OpenConnectionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Dict_PipingBend ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Dict_PipingBend OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return _mapper.Map<DspSpmcDictPipingBendDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateDspSpmcDictPipingBendDto dto)
        {
            var entity = await _context.DspSpmcDictPipingBends.FindAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.DspSpmcDictPipingBends.FindAsync(id);
            if (entity == null) return false;

            _context.DspSpmcDictPipingBends.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
