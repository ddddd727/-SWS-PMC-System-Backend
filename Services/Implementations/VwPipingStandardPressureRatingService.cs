using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class VwPipingStandardPressureRatingService : IVwPipingStandardPressureRatingService
    {
        private readonly PmcContext _context;

        public VwPipingStandardPressureRatingService(PmcContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync()
        {
            return await _context.VwPipingStandardPressureRatings
                .Select(x => new PipingStandardDto
                {
                    PipingStandardCode = x.PipingStandardCode,
                    PipeStandDesc = x.PipingStandardDesc, // Mapping PipingStandardDesc to PipeStandDesc
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<PressureRatingDto>> GetPressureRatingsByPipingClAsync(int geometricIndustryStandardCl)
        {
            return await _context.VwPipingStandardPressureRatings
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
