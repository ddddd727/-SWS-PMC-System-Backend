using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCommonPlainPipingGenericDataController : ApiControllerBase
    {
        private readonly IS3dCommonPlainPipingGenericDataService _service;

        public S3dCommonPlainPipingGenericDataController(IS3dCommonPlainPipingGenericDataService service)
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
        public async Task<IActionResult> Create([FromBody] CreateS3dCommonPlainPipingGenericDataDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Success(result, "创建成功");
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ApiErrorCode.DuplicateResource, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateS3dCommonPlainPipingGenericDataDto dto)
        {
            try
            {
                var success = await _service.UpdateAsync(dto);
                if (!success)
                {
                    return Fail(ApiErrorCode.ResourceNotFound, "更新失败，未找到记录");
                }
                return Success("更新成功");
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ApiErrorCode.DuplicateResource, ex.Message);
            }
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
