using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeC1c2ViewController : ControllerBase
    {
        private readonly IS3dCodeC1c2ViewService _service;

        public S3dCodeC1c2ViewController(IS3dCodeC1c2ViewService service)
        {
            _service = service;
        }

        [HttpGet("{ruleName}")]
        public async Task<ActionResult<IEnumerable<S3dCodeC1c2Dto>>> GetByRuleName(string ruleName)
        {
            var result = await _service.GetByRuleNameAsync(ruleName);
            return Ok(result);
        }
    }
}
