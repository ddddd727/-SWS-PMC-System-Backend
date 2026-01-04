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
            _context.DspSpmcDictPipingBends.Add(entity);
            await _context.SaveChangesAsync();
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
