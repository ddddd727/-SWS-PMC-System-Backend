using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class VwPipingStandardMaterialsGradeService : IVwPipingStandardMaterialsGradeService
    {
        private readonly PmcContext _context;

        public VwPipingStandardMaterialsGradeService(PmcContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync()
        {
            return await _context.VwPipingStandardMaterialsGrades
                .Select(x => new PipingStandardDto
                {
                    PipingStandardCode = x.PipingStandardCode,
                    PipeStandDesc = x.PipeStandDesc,
                    GeometricIndustryStandardCl = x.GeometricIndustryStandardCl
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<MaterialsGradeDto>> GetMaterialsGradesByPipingClAsync(int geometricIndustryStandardCl)
        {
            return await _context.VwPipingStandardMaterialsGrades
                .Where(x => x.GeometricIndustryStandardCl == geometricIndustryStandardCl)
                .Select(x => new MaterialsGradeDto
                {
                    MaterialsGradeCode = x.MaterialsGradeCode,
                    MaterialsGradeDesc = x.MaterialsGradeDesc,
                    MaterialsGradeCl = x.MaterialsGradeCl
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
