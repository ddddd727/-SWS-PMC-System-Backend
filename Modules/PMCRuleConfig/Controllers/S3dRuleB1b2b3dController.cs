using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dRuleB1b2b3dController : ControllerBase
    {
        private readonly IS3dRuleB1b2b3dService _service;

        public S3dRuleB1b2b3dController(IS3dRuleB1b2b3dService service)
        {
            _service = service;
        }

        [HttpGet("rule-names")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllRuleNames()
        {
            var result = await _service.GetAllRuleNamesAsync();
            return Ok(result);
        }

        [HttpGet("{ruleName}")]
        public async Task<ActionResult<IEnumerable<S3dRuleB1b2b3dDto>>> GetByRuleName(string ruleName)
        {
            var result = await _service.GetByRuleNameAsync(ruleName);
            return Ok(result);
        }

        [HttpPost("{ruleName}")]
        public async Task<IActionResult> SaveOrUpdate(string ruleName, [FromBody] List<S3dRuleB1b2b3dDto> dtos)
        {
            await _service.SaveOrUpdateByRuleNameAsync(ruleName, dtos);
            return Ok();
        }

        [HttpDelete("{ruleName}")]
        public async Task<IActionResult> Delete(string ruleName)
        {
            await _service.DeleteByRuleNameAsync(ruleName);
            return NoContent();
        }

        [HttpPut("update-name")]
        public async Task<IActionResult> UpdateRuleName(string oldRuleName, string newRuleName)
        {
            await _service.UpdateRuleNameAsync(oldRuleName, newRuleName);
            return Ok();
        }
    }
}
