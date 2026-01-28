using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dRuleAb2b3c2Service
    {
        Task<IEnumerable<string>> GetAllRuleNamesAsync();
        Task<IEnumerable<S3dRuleAb2b3c2Dto>> GetByRuleNameAsync(string ruleName);
        Task SaveOrUpdateByRuleNameAsync(string ruleName, List<S3dRuleAb2b3c2Dto> dtos);
        Task DeleteByRuleNameAsync(string ruleName);
        Task UpdateRuleNameAsync(string oldRuleName, string newRuleName);
    }
}