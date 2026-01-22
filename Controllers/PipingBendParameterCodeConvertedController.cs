using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipingBendParameterCodeConvertedController : ApiControllerBase
    {
        private readonly IPipingBendParameterCodeConvertedService _service;

        public PipingBendParameterCodeConvertedController(IPipingBendParameterCodeConvertedService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Success(result);
        }
    }
}

