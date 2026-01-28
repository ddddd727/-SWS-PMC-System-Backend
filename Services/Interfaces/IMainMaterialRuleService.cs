using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IMainMaterialRuleService
    {
        IEnumerable<string> GetRuleNames();
        IEnumerable<MainMaterialRuleDto> GetByRuleName(string ruleName);
    }
}
