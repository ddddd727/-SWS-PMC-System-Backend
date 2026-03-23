using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeB1b2b3dViewController : ControllerBase
    {
        private readonly IS3dCodeB1b2b3dViewService _service;

        public S3dCodeB1b2b3dViewController(IS3dCodeB1b2b3dViewService service)
        {
            _service = service;
        }

        [HttpGet("{ruleName}")]
        public async Task<ActionResult<IEnumerable<S3dCodeB1b2b3dDto>>> GetByRuleName(string ruleName)
        {
            var result = await _service.GetByRuleNameAsync(ruleName);
            return Ok(result);
        }
    }
}
