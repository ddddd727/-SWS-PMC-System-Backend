using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VwPipingStandardMaterialsGradeController : ControllerBase
    {
        private readonly IVwPipingStandardMaterialsGradeService _service;

        public VwPipingStandardMaterialsGradeController(IVwPipingStandardMaterialsGradeService service)
        {
            _service = service;
        }

        [HttpGet("piping-standards")]
        public async Task<ActionResult<IEnumerable<PipingStandardDto>>> GetPipingStandards()
        {
            var result = await _service.GetUniquePipingStandardsAsync();
            return Ok(result);
        }

        [HttpGet("materials-grades/{geometricIndustryStandardCl}")]
        public async Task<ActionResult<IEnumerable<MaterialsGradeDto>>> GetMaterialsGrades(int geometricIndustryStandardCl)
        {
            var result = await _service.GetMaterialsGradesByPipingClAsync(geometricIndustryStandardCl);
            return Ok(result);
        }
    }
}
