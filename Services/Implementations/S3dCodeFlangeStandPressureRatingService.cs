using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeFlangeStandPressureRatingService : IS3dCodeFlangeStandPressureRatingService
    {
        private readonly SpecContext _context;

        public S3dCodeFlangeStandPressureRatingService(SpecContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlangeStandardDto>> GetUniqueFlangeStandardsAsync()
        {
            return await _context.S3dCodeFlangeStandPressureRatings
                .Select(x => new FlangeStandardDto
                {
                    FlangeStandardCode = x.FlangeStandardCode,
                    FlangeStandDesc = x.FlangeStandDesc,
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<PressureRatingDto>> GetPressureRatingsByFlangeClAsync(int geometricIndustryStandardCl)
        {
            return await _context.S3dCodeFlangeStandPressureRatings
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
