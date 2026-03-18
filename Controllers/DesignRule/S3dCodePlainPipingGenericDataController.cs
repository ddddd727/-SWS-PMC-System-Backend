using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodePlainPipingGenericDataController : ControllerBase
    {
        private readonly IS3dCodePlainPipingGenericDataService _service;

        public S3dCodePlainPipingGenericDataController(IS3dCodePlainPipingGenericDataService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<S3dCodePlainPipingGenericDataDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
