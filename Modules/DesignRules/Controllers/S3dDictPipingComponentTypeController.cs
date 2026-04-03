using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Modules.DesignRules.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dDictPipingComponentTypeController : ControllerBase
    {
        private readonly IS3dDictPipingComponentTypeService _service;

        public S3dDictPipingComponentTypeController(IS3dDictPipingComponentTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<S3dDictPipingComponentTypeDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
