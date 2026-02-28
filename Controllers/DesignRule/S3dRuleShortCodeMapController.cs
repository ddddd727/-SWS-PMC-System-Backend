using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Services.Interfaces;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dRuleShortCodeMapController : ApiControllerBase
    {
        private readonly IS3dRuleShortCodeMapService _service;

        public S3dRuleShortCodeMapController(IS3dRuleShortCodeMapService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Success(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateS3dRuleShortCodeMapDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Success(result, "创建成功");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateS3dRuleShortCodeMapDto dto)
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
