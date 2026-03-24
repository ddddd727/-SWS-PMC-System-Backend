using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IMainMaterialRuleService
    {
        IEnumerable<string> GetRuleNames();
        IEnumerable<MainMaterialRuleDto> GetByRuleName(string ruleName);
    }
}
