using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IPipeLimitRuleService
    {
        IEnumerable<PipeLimitRuleDto> GetAll();
        PipeLimitRuleDto? GetById(int id);
        IEnumerable<PipeLimitRuleDto> GetByRuleName(string ruleName);
        IEnumerable<string> GetRuleNames();
    }
}
