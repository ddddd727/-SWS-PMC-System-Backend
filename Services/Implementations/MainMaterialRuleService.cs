using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class MainMaterialRuleService : IMainMaterialRuleService
    {
        private readonly AppDbContext _context;

        public MainMaterialRuleService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetRuleNames()
        {
            return _context.S3dCodeB1b2b3ds
                .AsNoTracking()
                .Select(x => x.RuleName)
                .Where(x => x != null)
                .Distinct()
                .ToList()!;
        }

        public IEnumerable<MainMaterialRuleDto> GetByRuleName(string ruleName)
        {
            var entities = _context.S3dCodeB1b2b3ds
                .AsNoTracking()
                .Where(x => x.RuleName == ruleName)
                .ToList();

            return entities.Select(entity => new MainMaterialRuleDto
            {
                Id = entity.Id,
                RuleName = entity.RuleName,
                MaterialsCategoryCode = entity.MaterialsCategoryCode,
                PipingStandardCode = entity.PipingStandardCode,
                MaterialsGradeCode = entity.MaterialsGradeCode,
                ScheduleThicknessCode = entity.ScheduleThicknessCode
            }).ToList();
        }
    }
}
