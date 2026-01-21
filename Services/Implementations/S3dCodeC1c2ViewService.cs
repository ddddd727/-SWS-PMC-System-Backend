using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeC1c2ViewService : IS3dCodeC1c2ViewService
    {
        private readonly PmcContext _context;

        public S3dCodeC1c2ViewService(PmcContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<S3dCodeC1c2Dto>> GetByRuleNameAsync(string ruleName)
        {
            return await _context.S3dCodeC1c2s
                .Where(x => x.RuleName == ruleName)
                .Select(x => new S3dCodeC1c2Dto
                {
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl,
                    PressureRatingCl = x.PressureRatingCl,
                    RuleName = x.RuleName,
                    Status = x.Status,
                    FlangeStandardCode = x.FlangeStandardCode,
                    PressureRatingCode = x.PressureRatingCode
                })
                .ToListAsync();
        }
    }
}
