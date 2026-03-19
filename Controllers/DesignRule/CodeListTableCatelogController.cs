using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.DesignRule;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeListTableCatelogController : ControllerBase
    {
        private readonly ICodeListTableCatelogService _service;

        public CodeListTableCatelogController(ICodeListTableCatelogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CodeListTableCatelogDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CodeListTableCatelogDto>> Create([FromBody] CreateCodeListTableCatelogDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data");
            }

            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
    }
}
