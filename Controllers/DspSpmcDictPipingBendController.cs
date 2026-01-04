using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DspSpmcDictPipingBendController : ApiControllerBase
    {
        private readonly IDspSpmcDictPipingBendService _service;

        public DspSpmcDictPipingBendController(IDspSpmcDictPipingBendService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllListAsync();
            return Success(result);
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return Fail(ApiErrorCode.ResourceNotFound, "未找到记录");
            }
            return Success(result);
        }

        /// <summary>
        /// 创建新记录
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDspSpmcDictPipingBendDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Success(result, "创建成功");
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDspSpmcDictPipingBendDto dto)
        {
            var success = await _service.UpdateAsync(dto);
            if (!success)
            {
                return Fail(ApiErrorCode.ResourceNotFound, "更新失败，未找到记录");
            }
            return Success("更新成功");
        }

        /// <summary>
        /// 删除记录
        /// </summary>
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
