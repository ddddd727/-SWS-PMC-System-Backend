using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dRuleC1c2Service
    {
        Task<IEnumerable<string>> GetAllRuleNamesAsync();
        Task<IEnumerable<S3dRuleC1c2Dto>> GetByRuleNameAsync(string ruleName);
        Task SaveOrUpdateByRuleNameAsync(string ruleName, List<S3dRuleC1c2Dto> dtos);
        Task DeleteByRuleNameAsync(string ruleName);
        Task UpdateRuleNameAsync(string oldRuleName, string newRuleName);
    }
}
