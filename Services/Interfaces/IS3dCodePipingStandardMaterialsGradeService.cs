using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodePipingStandardMaterialsGradeService
    {
        Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync();
        Task<IEnumerable<MaterialsGradeDto>> GetMaterialsGradesByPipingClAsync(int geometricIndustryStandardCl);
    }
}
