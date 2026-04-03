using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodePipingStandardMaterialsGradeService : IS3dCodePipingStandardMaterialsGradeService
    {
        private readonly AppDbContext _context;

        public S3dCodePipingStandardMaterialsGradeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync()
        {
            return await _context.S3dCodePipingStandardMaterialsGrades
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
            return await _context.S3dCodePipingStandardMaterialsGrades
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
