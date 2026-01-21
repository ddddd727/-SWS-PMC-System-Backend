using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodeAb2b3c2ViewService
    {
        Task<IEnumerable<S3dCodeAb2b3c2Dto>> GetByRuleNameAsync(string ruleName);
    }
}
