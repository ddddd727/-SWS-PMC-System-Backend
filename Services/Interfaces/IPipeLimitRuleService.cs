using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IPipeLimitRuleService
    {
        IEnumerable<PipeLimitRuleDto> GetAll();
        PipeLimitRuleDto? GetById(int id);
        IEnumerable<PipeLimitRuleDto> GetByRuleName(string ruleName);
        IEnumerable<string> GetRuleNames();
    }
}
