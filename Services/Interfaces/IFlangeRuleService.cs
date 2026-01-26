using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IFlangeRuleService
    {
        IEnumerable<string> GetRuleNames();
        IEnumerable<FlangeRuleDto> GetByRuleName(string ruleName);
    }
}
