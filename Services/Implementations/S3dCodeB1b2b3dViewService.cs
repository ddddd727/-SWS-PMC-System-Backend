using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeB1b2b3dViewService : IS3dCodeB1b2b3dViewService
    {
        private readonly AppDbContext _context;

        public S3dCodeB1b2b3dViewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<S3dCodeB1b2b3dDto>> GetByRuleNameAsync(string ruleName)
        {
            return await _context.S3dCodeB1b2b3ds
                .Where(x => x.RuleName == ruleName)
                .Select(x => new S3dCodeB1b2b3dDto
                {
                    MaterialsCategoryCl = x.MaterialsCategoryCl,
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl,
                    MaterialsGradeCl = x.MaterialsGradeCl,
                    ScheduleThicknessCl = x.ScheduleThicknessCl,
                    RuleName = x.RuleName,
                    Status = x.Status,
                    MaterialsCategoryCode = x.MaterialsCategoryCode,
                    PipingStandardCode = x.PipingStandardCode,
                    MaterialsGradeCode = x.MaterialsGradeCode,
                    ScheduleThicknessCode = x.ScheduleThicknessCode
                })
                .ToListAsync();
        }
    }
}
