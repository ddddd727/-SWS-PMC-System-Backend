using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeMaterialsCategoryScheduleThicknessController : ControllerBase
    {
        private readonly IS3dCodeMaterialsCategoryScheduleThicknessService _service;

        public S3dCodeMaterialsCategoryScheduleThicknessController(IS3dCodeMaterialsCategoryScheduleThicknessService service)
        {
            _service = service;
        }

        [HttpGet("schedule-thicknesses/{materialsCategoryCl}")]
        public async Task<ActionResult<IEnumerable<ScheduleThicknessDto>>> GetScheduleThicknesses(int materialsCategoryCl)
        {
            var result = await _service.GetScheduleThicknessesByMaterialsCategoryClAsync(materialsCategoryCl);
            return Ok(result);
        }
    }
}
