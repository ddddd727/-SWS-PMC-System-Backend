using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3dCodeWallThicknessController : ApiControllerBase
    {
        private readonly IS3dCodeWallThicknessService _service;

        public S3dCodeWallThicknessController(IS3dCodeWallThicknessService service)
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
