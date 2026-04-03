using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dRuleC1c2Controller : ControllerBase
    {
        private readonly IS3dRuleC1c2Service _service;

        public S3dRuleC1c2Controller(IS3dRuleC1c2Service service)
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
        public async Task<ActionResult<IEnumerable<S3dRuleC1c2Dto>>> GetByRuleName(string ruleName)
        {
            var result = await _service.GetByRuleNameAsync(ruleName);
            return Ok(result);
        }

        [HttpPost("{ruleName}")]
        public async Task<IActionResult> SaveOrUpdate(string ruleName, [FromBody] List<S3dRuleC1c2Dto> dtos)
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
