using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeMaterialsCategoryPipingStandardService : IS3dCodeMaterialsCategoryPipingStandardService
    {
        private readonly PmcContext _context;

        public S3dCodeMaterialsCategoryPipingStandardService(PmcContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaterialsCategoryDto>> GetUniqueMaterialsCategoriesAsync()
        {
            return await _context.S3dCodeMaterialsCategoryPipingStandards
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
            return await _context.S3dCodeMaterialsCategoryPipingStandards
                .Where(x => x.MaterialsCategoryCl == materialsCategoryCl)
                .Select(x => new PipingStandardDto
                {
                    PipingStandardCode = x.PipingStandardCode,
                    PipeStandDesc = x.PipeStandDesc,
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl
                })
                .Distinct() 
                .ToListAsync();
        }
    }
}
