using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodePipingStandardPressureRatingController : ControllerBase
    {
        private readonly IS3dCodePipingStandardPressureRatingService _service;

        public S3dCodePipingStandardPressureRatingController(IS3dCodePipingStandardPressureRatingService service)
        {
            _service = service;
        }

        [HttpGet("piping-standards")]
        public async Task<ActionResult<IEnumerable<PipingStandardDto>>> GetPipingStandards()
        {
            var result = await _service.GetUniquePipingStandardsAsync();
            return Ok(result);
        }

        [HttpGet("pressure-ratings/{geometricIndustryStandardCl}")]
        public async Task<ActionResult<IEnumerable<PressureRatingDto>>> GetPressureRatings(int geometricIndustryStandardCl)
        {
            var result = await _service.GetPressureRatingsByPipingClAsync(geometricIndustryStandardCl);
            return Ok(result);
        }
    }
}
