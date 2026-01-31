using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCommonCodeListValueController : ControllerBase
    {
        private readonly IS3dCommonCodeListValueService _service;

        public S3dCommonCodeListValueController(IS3dCommonCodeListValueService service)
        {
            _service = service;
        }

        [HttpGet("OPmaterialscategory")]
        public async Task<ActionResult<IEnumerable<S3dCommonCodeListValueDto>>> GetMaterialsCategory()
        {
            var result = await _service.GetMaterialsCategoryAsync();
            return Ok(result);
        }

        [HttpGet("OPScheduleThickness")]
        public async Task<ActionResult<IEnumerable<S3dCommonCodeListValueDto>>> GetScheduleThickness()
        {
            var result = await _service.GetScheduleThicknessAsync();
            return Ok(result);
        }

        [HttpGet("OPEndStandard")]
        public async Task<ActionResult<IEnumerable<S3dCommonCodeListValueDto>>> GetEndStandard()
        {
            var result = await _service.GetEndStandardAsync();
            return Ok(result);
        }

        [HttpGet("options/{tableName}")]
        public async Task<IActionResult> GetOptions(string tableName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                    return BadRequest("Table name cannot be empty");

                // 直接去查库，不关心你是哪个业务模块的
                var result = await _service.GetOptionsAsync(tableName);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
