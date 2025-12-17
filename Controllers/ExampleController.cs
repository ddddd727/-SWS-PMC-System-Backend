using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : Controller
    {
        private readonly IExampleService _service;

        public ExampleController(IExampleService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            var data = _service.GetExampleData();
            return Ok(data);
        }

        [HttpPost("post")]
        public IActionResult Post([FromBody] ExampleDto input)
        {
            input.Message += "（Processed by backend）";
            input.Timestamp = DateTime.Now;
            return Ok(input);
        }

        [HttpPost("posttodb")]
        public IActionResult PostToDb([FromBody] ExampleDto input)
        {
            try
            {
                _service.SaveExample(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("getfromdb")]
        public IActionResult GetFromDb()
        {
            var data = _service.GetDbData();
            return Ok(data);
        }
    }
}
