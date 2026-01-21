using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeAb2b3c2ViewService : IS3dCodeAb2b3c2ViewService
    {
        private readonly PmcContext _context;

        public S3dCodeAb2b3c2ViewService(PmcContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<S3dCodeAb2b3c2Dto>> GetByRuleNameAsync(string ruleName)
        {
            return await _context.S3dCodeAb2b3c2s
                .Where(x => x.RuleName == ruleName)
                .Select(x => new S3dCodeAb2b3c2Dto
                {
                    PipingClassCl = x.PipingClassCl,
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl,
                    MaterialsGradeCl = x.MaterialsGradeCl,
                    PressureRatingCl = x.PressureRatingCl,
                    RuleName = x.RuleName,
                    Status = x.Status,
                    PipingClassCode = x.PipingClassCode,
                    PipingStandardCode = x.PipingStandardCode,
                    MaterialsGradeCode = x.MaterialsGradeCode,
                    PressureRatingCode = x.PressureRatingCode
                })
                .ToListAsync();
        }
    }
}
