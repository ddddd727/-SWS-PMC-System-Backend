using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeFlangeStandPressureRatingController : ControllerBase
    {
        private readonly IS3dCodeFlangeStandPressureRatingService _service;

        public S3dCodeFlangeStandPressureRatingController(IS3dCodeFlangeStandPressureRatingService service)
        {
            _service = service;
        }

        [HttpGet("flange-standards")]
        public async Task<ActionResult<IEnumerable<FlangeStandardDto>>> GetFlangeStandards()
        {
            var result = await _service.GetUniqueFlangeStandardsAsync();
            return Ok(result);
        }

        [HttpGet("pressure-ratings/{geometricIndustryStandardCl}")]
        public async Task<ActionResult<IEnumerable<PressureRatingDto>>> GetPressureRatings(int geometricIndustryStandardCl)
        {
            var result = await _service.GetPressureRatingsByFlangeClAsync(geometricIndustryStandardCl);
            return Ok(result);
        }
    }
}
