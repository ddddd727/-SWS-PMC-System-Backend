using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodePipingClassViewController : ControllerBase
    {
        private readonly IS3dCodePipingClassViewService _service;

        public S3dCodePipingClassViewController(IS3dCodePipingClassViewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<S3dCodePipingClassDto>>> Get()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}

