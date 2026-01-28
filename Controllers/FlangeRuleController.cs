using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/pmc/rules/flange")]
    public class FlangeRuleController : ApiControllerBase
    {
        private readonly IFlangeRuleService _service;

        public FlangeRuleController(IFlangeRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetRuleNames()
        {
            var names = _service.GetRuleNames();
            return Success(names, "查询法兰规则名称成功");
        }

        [HttpGet("{ruleName}")]
        public IActionResult GetByRuleName(string ruleName)
        {
            var data = _service.GetByRuleName(ruleName);
            return Success(data, "查询法兰规则数据成功");
        }
    }
}
