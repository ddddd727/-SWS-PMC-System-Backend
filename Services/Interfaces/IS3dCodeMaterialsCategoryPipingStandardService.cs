using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodeMaterialsCategoryPipingStandardService
    {
        Task<IEnumerable<MaterialsCategoryDto>> GetUniqueMaterialsCategoriesAsync();
        Task<IEnumerable<PipingStandardDto>> GetPipingStandardsByCategoryClAsync(int materialsCategoryCl);
    }
}
