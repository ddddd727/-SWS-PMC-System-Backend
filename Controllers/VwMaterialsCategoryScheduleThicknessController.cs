using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VwMaterialsCategoryScheduleThicknessController : ControllerBase
    {
        private readonly IVwMaterialsCategoryScheduleThicknessService _service;

        public VwMaterialsCategoryScheduleThicknessController(IVwMaterialsCategoryScheduleThicknessService service)
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

