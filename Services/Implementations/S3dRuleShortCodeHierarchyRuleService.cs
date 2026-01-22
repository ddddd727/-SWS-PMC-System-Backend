using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dRuleShortCodeHierarchyRuleService : IS3dRuleShortCodeHierarchyRuleService
    {
        private readonly PmcContext _context;
        private readonly IMapper _mapper;

        public S3dRuleShortCodeHierarchyRuleService(PmcContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dRuleShortCodeHierarchyRuleDto>> GetAllListAsync()
        {
            var items = await _context.S3dRuleShortCodeHierarchyRules.ToListAsync();
            return _mapper.Map<List<S3dRuleShortCodeHierarchyRuleDto>>(items);
        }

        public async Task<S3dRuleShortCodeHierarchyRuleDto> CreateAsync(CreateS3dRuleShortCodeHierarchyRuleDto dto)
        {
            var entity = _mapper.Map<S3dRuleShortCodeHierarchyRule>(dto);
            
            // Manual ID calculation
            var maxId = await _context.S3dRuleShortCodeHierarchyRules.MaxAsync(e => (int?)e.Id) ?? 0;
            entity.Id = maxId + 1;
            
            _context.S3dRuleShortCodeHierarchyRules.Add(entity);

            await _context.Database.OpenConnectionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Rule_ShortCodeHierarchyRule ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT S3D_Rule_ShortCodeHierarchyRule OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return _mapper.Map<S3dRuleShortCodeHierarchyRuleDto>(entity);
        }

        public async Task<bool> UpdateAsync(UpdateS3dRuleShortCodeHierarchyRuleDto dto)
        {
            var entity = await _context.S3dRuleShortCodeHierarchyRules.FindAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.S3dRuleShortCodeHierarchyRules.FindAsync(id);
            if (entity == null) return false;

            _context.S3dRuleShortCodeHierarchyRules.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
