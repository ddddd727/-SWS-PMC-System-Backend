using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeMaterialsCategoryPipingStandardController : ControllerBase
    {
        private readonly IS3dCodeMaterialsCategoryPipingStandardService _service;

        public S3dCodeMaterialsCategoryPipingStandardController(IS3dCodeMaterialsCategoryPipingStandardService service)
        {
            _service = service;
        }

        [HttpGet("materials-categories")]
        public async Task<ActionResult<IEnumerable<MaterialsCategoryDto>>> GetMaterialsCategories()
        {
            var result = await _service.GetUniqueMaterialsCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("piping-standards/{materialsCategoryCl}")]
        public async Task<ActionResult<IEnumerable<PipingStandardDto>>> GetPipingStandards(int materialsCategoryCl)
        {
            var result = await _service.GetPipingStandardsByCategoryClAsync(materialsCategoryCl);
            return Ok(result);
        }
    }
}
