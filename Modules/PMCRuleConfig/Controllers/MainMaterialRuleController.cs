using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [ApiController]
    [Route("api/pmc/rules/main-material")]
    public class MainMaterialRuleController : ApiControllerBase
    {
        private readonly IMainMaterialRuleService _service;

        public MainMaterialRuleController(IMainMaterialRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetRuleNames()
        {
            var names = _service.GetRuleNames();
            return Success(names, "查询主材料规则名称成功");
        }

        [HttpGet("{ruleName}")]
        public IActionResult GetByRuleName(string ruleName)
        {
            var data = _service.GetByRuleName(ruleName);
            return Success(data, "查询主材料规则数据成功");
        }
    }
}
