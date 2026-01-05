using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/rules/ab2b3c2")]
    public class RuleAb2b3c2Controller : Controller
    {
        private readonly IRuleAb2b3c2Service _service;

        public RuleAb2b3c2Controller(IRuleAb2b3c2Service service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}
