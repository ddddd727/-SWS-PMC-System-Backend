using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class VwMaterialsCategoryPipingStandardService : IVwMaterialsCategoryPipingStandardService
    {
        private readonly PmcContext _context;

        public VwMaterialsCategoryPipingStandardService(PmcContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaterialsCategoryDto>> GetUniqueMaterialsCategoriesAsync()
        {
            return await _context.VwMaterialsCategoryPipingStandards
                .Select(x => new MaterialsCategoryDto
                {
                    MaterialsCategoryCode = x.MaterialsCategoryCode,
                    MaterialsCategoryDesc = x.MaterialsCategoryDesc,
                    MaterialsCategoryCl = x.MaterialsCategoryCl
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<PipingStandardDto>> GetPipingStandardsByCategoryClAsync(int materialsCategoryCl)
        {
            return await _context.VwMaterialsCategoryPipingStandards
                .Where(x => x.MaterialsCategoryCl == materialsCategoryCl)
                .Select(x => new PipingStandardDto
                {
                    PipingStandardCode = x.PipingStandardCode,
                    PipeStandDesc = x.PipeStandDesc,
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl
                })
                .Distinct() // Adding Distinct here as well in case there are duplicates for the same standard within the category
                .ToListAsync();
        }
    }
}
