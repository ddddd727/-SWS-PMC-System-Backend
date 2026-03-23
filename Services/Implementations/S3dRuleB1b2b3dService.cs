using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dRuleB1b2b3dService : IS3dRuleB1b2b3dService
    {
        private readonly SpecContext _context;
        private readonly IMapper _mapper;

        public S3dRuleB1b2b3dService(SpecContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetAllRuleNamesAsync()
        {
            return await _context.S3dRuleB1b2b3ds
                .Select(x => x.RuleName)
                .Where(rn => rn != null)
                .Distinct()
                .ToListAsync() as IEnumerable<string>;
        }

        public async Task<IEnumerable<S3dRuleB1b2b3dDto>> GetByRuleNameAsync(string ruleName)
        {
            var entities = await _context.S3dRuleB1b2b3ds
                .Where(x => x.RuleName == ruleName)
                .ToListAsync();
            return _mapper.Map<IEnumerable<S3dRuleB1b2b3dDto>>(entities);
        }

        public async Task SaveOrUpdateByRuleNameAsync(string ruleName, List<S3dRuleB1b2b3dDto> dtos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingEntities = await _context.S3dRuleB1b2b3ds
                    .Where(x => x.RuleName == ruleName)
                    .ToListAsync();
                
                if (existingEntities.Any())
                {
                    _context.S3dRuleB1b2b3ds.RemoveRange(existingEntities);
                }

                var newEntities = _mapper.Map<List<S3dRuleB1b2b3d>>(dtos);
                foreach (var entity in newEntities)
                {
                    entity.RuleName = ruleName;
                }
                
                await _context.S3dRuleB1b2b3ds.AddRangeAsync(newEntities);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteByRuleNameAsync(string ruleName)
        {
            var entities = await _context.S3dRuleB1b2b3ds
                .Where(x => x.RuleName == ruleName)
                .ToListAsync();

            if (entities.Any())
            {
                _context.S3dRuleB1b2b3ds.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRuleNameAsync(string oldRuleName, string newRuleName)
        {
            var entities = await _context.S3dRuleB1b2b3ds
                .Where(x => x.RuleName == oldRuleName)
                .ToListAsync();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    entity.RuleName = newRuleName;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
