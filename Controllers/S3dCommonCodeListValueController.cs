using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.DesignRules;
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
    }
}
