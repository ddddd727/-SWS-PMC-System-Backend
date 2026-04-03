using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.DesignRules.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodePipingBendParameterController : ControllerBase
    {
        private readonly IS3dCodePipingBendParameterService _service;

        public S3dCodePipingBendParameterController(IS3dCodePipingBendParameterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<S3dCodePipingBendParameterDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
