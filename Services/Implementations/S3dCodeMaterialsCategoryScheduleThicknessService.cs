using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeMaterialsCategoryScheduleThicknessService : IS3dCodeMaterialsCategoryScheduleThicknessService
    {
        private readonly SpecContext _context;

        public S3dCodeMaterialsCategoryScheduleThicknessService(SpecContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleThicknessDto>> GetScheduleThicknessesByMaterialsCategoryClAsync(int materialsCategoryCl)
        {
            return await _context.S3dCodeMaterialsCategoryScheduleThicknesses
                .Where(x => x.MaterialsCategoryCl == materialsCategoryCl)
                .Select(x => new ScheduleThicknessDto
                {
                    ScheduleThicknessCode = x.ScheduleThicknessCode,
                    ScheduleThicknessDesc = x.ScheduleThicknessDesc,
                    ScheduleThicknessCl = x.ScheduleThicknessCl
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
