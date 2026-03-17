using System.Collections.Generic;
using System.Threading.Tasks;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dDictPipingComponentTypeService
    {
        Task<IEnumerable<S3dDictPipingComponentTypeDto>> GetAllAsync();
    }
}
