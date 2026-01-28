using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dRuleAb2b3c2Controller : ApiControllerBase
    {
        private readonly IS3dRuleAb2b3c2Service _service;

        public S3dRuleAb2b3c2Controller(IS3dRuleAb2b3c2Service service)
        {
            _service = service;
        }

        [HttpGet("rule-names")]
        public async Task<IActionResult> GetAllRuleNames()
        {
            var result = await _service.GetAllRuleNamesAsync();
            return Success(result);
        }

        [HttpGet("{ruleName}")]
        public async Task<IActionResult> GetByRuleName(string ruleName)
        {
            var result = await _service.GetByRuleNameAsync(ruleName);
            return Success(result);
        }

        [HttpPost("{ruleName}")]
        public async Task<IActionResult> SaveOrUpdate(string ruleName, [FromBody] List<S3dRuleAb2b3c2Dto> dtos)
        {
            await _service.SaveOrUpdateByRuleNameAsync(ruleName, dtos);
            return Success("保存成功");
        }

        [HttpDelete("{ruleName}")]
        public async Task<IActionResult> DeleteByRuleName(string ruleName)
        {
            await _service.DeleteByRuleNameAsync(ruleName);
            return Success("删除成功");
        }

        [HttpPut("update-name")]
        public async Task<IActionResult> UpdateRuleName(string oldRuleName, string newRuleName)
        {
            await _service.UpdateRuleNameAsync(oldRuleName, newRuleName);
            return Success("规则名称更新成功");
        }
    }
}