using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Services.Impletation
{
    public class PipeLimitRuleService : IPipeLimitRuleService
    {
        private readonly PmcTestContext _context;
        private readonly IMapper _mapper;

        public PipeLimitRuleService(PmcTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<PipeLimitRuleDto> GetAll()
        {
            var query = _context.VwS3dRuleAb2b3c2WithCodes.AsNoTracking();
            return query.Select(x => new PipeLimitRuleDto
            {
                Id = x.Id,
                RuleName = x.RuleName,
                PipingClassCode = x.PipingClassCode,
                PipingStandardCode = x.PipingStandardCode,
                MaterialsGradeCode = x.MaterialsGradeCode,
                PressureRatingCode = x.PressureRatingCode
            }).ToList();
        }

        public PipeLimitRuleDto? GetById(int id)
        {
            var entity = _context.VwS3dRuleAb2b3c2WithCodes
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            if (entity == null) return null;
            return new PipeLimitRuleDto
            {
                Id = entity.Id,
                RuleName = entity.RuleName,
                PipingClassCode = entity.PipingClassCode,
                PipingStandardCode = entity.PipingStandardCode,
                MaterialsGradeCode = entity.MaterialsGradeCode,
                PressureRatingCode = entity.PressureRatingCode
            };
        }

        public IEnumerable<PipeLimitRuleDto> GetByRuleName(string ruleName)
        {
            var entities = _context.VwS3dRuleAb2b3c2WithCodes
                .AsNoTracking()
                .Where(x => x.RuleName == ruleName)
                .ToList();

            return entities.Select(entity => new PipeLimitRuleDto
            {
                Id = entity.Id,
                RuleName = entity.RuleName,
                PipingClassCode = entity.PipingClassCode,
                PipingStandardCode = entity.PipingStandardCode,
                MaterialsGradeCode = entity.MaterialsGradeCode,
                PressureRatingCode = entity.PressureRatingCode
            }).ToList();
        }

        public IEnumerable<string> GetRuleNames()
        {
            return _context.VwS3dRuleAb2b3c2WithCodes
                .AsNoTracking()
                .Select(x => x.RuleName)
                .Where(x => x != null)
                .Distinct()
                .ToList()!;
        }
    }
}
