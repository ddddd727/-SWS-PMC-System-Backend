using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WallThicknessCodeConvertedController : ApiControllerBase
    {
        private readonly IWallThicknessCodeConvertedService _service;

        public WallThicknessCodeConvertedController(IWallThicknessCodeConvertedService service)
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
