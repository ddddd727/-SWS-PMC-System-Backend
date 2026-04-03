using System.Collections.Generic;
using System.Threading.Tasks;
using PMCSystem_Backend.Modules.DesignRules.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dDictPipingComponentTypeService
    {
        Task<IEnumerable<S3dDictPipingComponentTypeDto>> GetAllAsync();
    }
}
