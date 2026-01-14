using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Controllers;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/pmc/rules/pipe-limit")]
    public class PipeLimitRuleController : ApiControllerBase
    {
        private readonly IPipeLimitRuleService _service;

        public PipeLimitRuleController(IPipeLimitRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetRuleNames()
        {
            var names = _service.GetRuleNames();
            return Success(names, "查询规则名称成功");
        }

        [HttpGet("{ruleName}")]
        public IActionResult GetByRuleName(string ruleName)
        {
            var data = _service.GetByRuleName(ruleName);
            // Even if empty, we return 200 with empty list
            return Success(data, "查询规则数据成功");
        }
    }
}

