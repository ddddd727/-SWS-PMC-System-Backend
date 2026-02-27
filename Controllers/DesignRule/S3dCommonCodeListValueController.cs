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

        [HttpGet("OPGeometricIndustryStandard")]
        public async Task<ActionResult<IEnumerable<S3dCommonCodeListValueDto>>> GetGeometricIndustryStandard()
        {
            var result = await _service.GetGeometricIndustryStandardAsync();
            return Ok(result);
        }

        [HttpGet("OPMaterialsGrade")]
        public async Task<ActionResult<IEnumerable<S3dCommonCodeListValueDto>>> GetMaterialsGrade()
        {
            var result = await _service.GetMaterialsGradeAsync();
            return Ok(result);
        }

        [HttpGet("ShortCodeHierarchyClass")]
        public async Task<ActionResult<IEnumerable<S3dCommonCodeListValueDto>>> GetShortCodeHierarchyClass()
        {
            var result = await _service.GetShortCodeHierarchyClassAsync();
            return Ok(result);
        }
    }
}
