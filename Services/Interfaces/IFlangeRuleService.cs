using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IFlangeRuleService
    {
        IEnumerable<string> GetRuleNames();
        IEnumerable<FlangeRuleDto> GetByRuleName(string ruleName);
    }
}
