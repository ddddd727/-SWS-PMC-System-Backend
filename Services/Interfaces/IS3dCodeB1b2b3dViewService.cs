using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodeB1b2b3dViewService
    {
        Task<IEnumerable<S3dCodeB1b2b3dDto>> GetByRuleNameAsync(string ruleName);
    }
}
