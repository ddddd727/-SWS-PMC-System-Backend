using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Services.Impletation
{
    public class FlangeRuleService : IFlangeRuleService
    {
        private readonly PmcContextLr _context;

        public FlangeRuleService(PmcContextLr context)
        {
            _context = context;
        }

        public IEnumerable<string> GetRuleNames()
        {
            return _context.S3dCodeC1c2s
                .AsNoTracking()
                .Select(x => x.RuleName)
                .Where(x => x != null)
                .Distinct()
                .ToList()!;
        }

        public IEnumerable<FlangeRuleDto> GetByRuleName(string ruleName)
        {
            var entities = _context.S3dCodeC1c2s
                .AsNoTracking()
                .Where(x => x.RuleName == ruleName)
                .ToList();

            return entities.Select(entity => new FlangeRuleDto
            {
                Id = entity.Id,
                RuleName = entity.RuleName,
                FlangeStandardCode = entity.FlangeStandardCode,
                PressureRatingCode = entity.PressureRatingCode
            }).ToList();
        }
    }
}
