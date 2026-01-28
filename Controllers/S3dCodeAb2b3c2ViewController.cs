using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeAb2b3c2ViewController : ControllerBase
    {
        private readonly IS3dCodeAb2b3c2ViewService _service;

        public S3dCodeAb2b3c2ViewController(IS3dCodeAb2b3c2ViewService service)
        {
            _service = service;
        }

        [HttpGet("{ruleName}")]
        public async Task<ActionResult<IEnumerable<S3dCodeAb2b3c2Dto>>> GetByRuleName(string ruleName)
        {
            var result = await _service.GetByRuleNameAsync(ruleName);
            return Ok(result);
        }
    }
}
