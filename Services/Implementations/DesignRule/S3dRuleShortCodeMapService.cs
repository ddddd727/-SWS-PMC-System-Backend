using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Modules.DesignRules.Entities;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dRuleShortCodeMapService : IS3dRuleShortCodeMapService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public S3dRuleShortCodeMapService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<S3dRuleShortCodeMapDto>> GetAllAsync()
        {
            var entities = await _context.S3dRuleShortCodeMaps.ToListAsync();
            return _mapper.Map<IEnumerable<S3dRuleShortCodeMapDto>>(entities);
        }

        public async Task<S3dRuleShortCodeMapDto> CreateAsync(CreateS3dRuleShortCodeMapDto dto)
        {
            var entity = _mapper.Map<S3dRuleShortCodeMap>(dto);
            _context.S3dRuleShortCodeMaps.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<S3dRuleShortCodeMapDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateS3dRuleShortCodeMapDto dto)
        {
            var entity = await _context.S3dRuleShortCodeMaps.FindAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.S3dRuleShortCodeMaps.FindAsync(id);
            if (entity == null) return false;

            _context.S3dRuleShortCodeMaps.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
