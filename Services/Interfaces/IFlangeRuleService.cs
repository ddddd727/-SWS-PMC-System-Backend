using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IFlangeRuleService
    {
        IEnumerable<string> GetRuleNames();
        IEnumerable<FlangeRuleDto> GetByRuleName(string ruleName);
    }
}
