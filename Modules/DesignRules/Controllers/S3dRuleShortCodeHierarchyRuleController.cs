using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared;
using PMCSystem_Backend.Shared.Enums;

namespace PMCSystem_Backend.Modules.DesignRules.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dRuleShortCodeHierarchyRuleController : ApiControllerBase
    {
        private readonly IS3dRuleShortCodeHierarchyRuleService _service;

        public S3dRuleShortCodeHierarchyRuleController(IS3dRuleShortCodeHierarchyRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllListAsync();
            return Success(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateS3dRuleShortCodeHierarchyRuleDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Success(result, "创建成功");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateS3dRuleShortCodeHierarchyRuleDto dto)
        {
            var success = await _service.UpdateAsync(dto);
            if (!success)
            {
                return Fail(ApiErrorCode.ResourceNotFound, "更新失败，未找到记录");
            }
            return Success("更新成功");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return Fail(ApiErrorCode.ResourceNotFound, "删除失败，未找到记录");
            }
            return Success("删除成功");
        }
    }
}
