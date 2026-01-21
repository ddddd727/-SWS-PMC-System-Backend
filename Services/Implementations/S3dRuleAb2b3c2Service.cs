using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dRuleAb2b3c2Service : IS3dRuleAb2b3c2Service
    {
        private readonly PmcContext _context;
        private readonly IMapper _mapper;

        public S3dRuleAb2b3c2Service(PmcContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetAllRuleNamesAsync()
        {
            return await _context.S3dRuleAb2b3c2s
                .Select(x => x.RuleName)
                .Where(rn => rn != null)
                .Distinct()
                .ToListAsync() as IEnumerable<string>;
        }

        public async Task<IEnumerable<S3dRuleAb2b3c2Dto>> GetByRuleNameAsync(string ruleName)
        {
            var entities = await _context.S3dRuleAb2b3c2s
                .Where(x => x.RuleName == ruleName)
                .ToListAsync();
            return _mapper.Map<IEnumerable<S3dRuleAb2b3c2Dto>>(entities);
        }

        public async Task SaveOrUpdateByRuleNameAsync(string ruleName, List<S3dRuleAb2b3c2Dto> dtos)
        {
            // Transaction is recommended for bulk operations to ensure data integrity
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Delete existing records with the same RuleName
                var existingEntities = await _context.S3dRuleAb2b3c2s
                    .Where(x => x.RuleName == ruleName)
                    .ToListAsync();
                
                if (existingEntities.Any())
                {
                    _context.S3dRuleAb2b3c2s.RemoveRange(existingEntities);
                }

                // 2. Add new records
                var newEntities = _mapper.Map<List<S3dRuleAb2b3c2>>(dtos);
                foreach (var entity in newEntities)
                {
                    // Ensure RuleName matches the provided one (optional, but good for consistency)
                    entity.RuleName = ruleName;
                }
                
                await _context.S3dRuleAb2b3c2s.AddRangeAsync(newEntities);
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
            var entities = await _context.S3dRuleAb2b3c2s
                .Where(x => x.RuleName == ruleName)
                .ToListAsync();

            if (entities.Any())
            {
                _context.S3dRuleAb2b3c2s.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRuleNameAsync(string oldRuleName, string newRuleName)
        {
            var entities = await _context.S3dRuleAb2b3c2s
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