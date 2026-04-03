using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dRuleB1b2b3dService
    {
        Task<IEnumerable<string>> GetAllRuleNamesAsync();
        Task<IEnumerable<S3dRuleB1b2b3dDto>> GetByRuleNameAsync(string ruleName);
        Task SaveOrUpdateByRuleNameAsync(string ruleName, List<S3dRuleB1b2b3dDto> dtos);
        Task DeleteByRuleNameAsync(string ruleName);
        Task UpdateRuleNameAsync(string oldRuleName, string newRuleName);
    }
}
