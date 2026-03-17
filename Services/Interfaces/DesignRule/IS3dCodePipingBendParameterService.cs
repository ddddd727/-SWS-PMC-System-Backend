using System.Collections.Generic;
using System.Threading.Tasks;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodePipingBendParameterService
    {
        Task<List<S3dCodePipingBendParameterDto>> GetAllAsync();
    }
}
