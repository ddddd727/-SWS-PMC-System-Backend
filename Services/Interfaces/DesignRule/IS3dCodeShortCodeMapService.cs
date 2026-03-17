using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodeShortCodeMapService
    {
        Task<List<S3dCodeShortCodeMapDto>> GetAllAsync();
    }
}
