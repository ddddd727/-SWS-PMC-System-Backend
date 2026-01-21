using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IVwPipingStandardMaterialsGradeService
    {
        Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync();
        Task<IEnumerable<MaterialsGradeDto>> GetMaterialsGradesByPipingClAsync(int geometricIndustryStandardCl);
    }
}
