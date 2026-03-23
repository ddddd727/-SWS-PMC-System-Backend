using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodePipingStandardPressureRatingService : IS3dCodePipingStandardPressureRatingService
    {
        private readonly SpecContext _context;

        public S3dCodePipingStandardPressureRatingService(SpecContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync()
        {
            return await _context.S3dCodePipingStandardPressureRatings
                .Select(x => new PipingStandardDto
                {
                    PipingStandardCode = x.PipingStandardCode,
                    PipeStandDesc = x.PipingStandardDesc, 
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<PressureRatingDto>> GetPressureRatingsByPipingClAsync(int geometricIndustryStandardCl)
        {
            return await _context.S3dCodePipingStandardPressureRatings
                .Where(x => x.GeometricIndustryStandardCl == geometricIndustryStandardCl)
                .Select(x => new PressureRatingDto
                {
                    PressureRatingCode = x.PressureRatingCode,
                    PressureRatingDesc = x.PressureRatingDesc,
                    PressureRatingCl = x.PressureRatingCl
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
