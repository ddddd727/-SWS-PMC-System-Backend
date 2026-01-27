using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodeC1c2ViewService
    {
        Task<IEnumerable<S3dCodeC1c2Dto>> GetByRuleNameAsync(string ruleName);
    }
}
