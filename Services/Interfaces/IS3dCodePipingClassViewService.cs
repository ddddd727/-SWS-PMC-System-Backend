using PMCSystem_Backend.Modules.DesignRules.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodePipingClassViewService
    {
        Task<IEnumerable<S3dCodePipingClassDto>> GetAllAsync();
    }
}

