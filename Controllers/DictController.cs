using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictController : ApiControllerBase
    {
        private readonly IDictService _service;

        public DictController(IDictService service)
        {
            _service = service;
        }

        // 查：GET /api/dict/std-series
        [HttpGet("{type}")]
        public async Task<IActionResult> GetTable(string type)
        {
            try
            {
                return Success(await _service.GetTableDataAsync(type));
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.ResourceNotFound, ex.Message);
            }
        }

        // 增：POST /api/dict/std-series
        [HttpPost("{type}")]
        public async Task<IActionResult> Add(string type, [FromBody] DictInputDto data)
        {
            try
            {
                await _service.AddAsync(type, data);
                return Success("添加成功");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BadRequest, ex.Message);
            }
        }

        // 改：PUT /api/dict/std-series/1
        [HttpPut("{type}/{id}")]
        public async Task<IActionResult> Update(string type, int id, [FromBody] DictInputDto data)
        {
            try
            {
                await _service.UpdateAsync(type, id, data);
                return Success("更新成功");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BadRequest, ex.Message);
            }
        }

        // 删：DELETE /api/dict/std-series/1
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            try
            {
                await _service.DeleteAsync(type, id);
                return Success("删除成功");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.SystemError, ex.Message);
            }
        }
    }
}