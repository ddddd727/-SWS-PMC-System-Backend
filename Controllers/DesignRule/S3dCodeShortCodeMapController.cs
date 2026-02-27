using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeShortCodeMapController : ControllerBase
    {
        private readonly IS3dCodeShortCodeMapService _service;

        public S3dCodeShortCodeMapController(IS3dCodeShortCodeMapService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<S3dCodeShortCodeMapDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
