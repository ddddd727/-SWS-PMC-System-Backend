using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;
using PMCSystem_Backend.Services.Interfaces.CodeListManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Modules.StandardComponents.Controllers
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





        [HttpGet("combined/{codeListTableName}")]
        public async Task<ActionResult<CodeListCombinedResponseDto>> GetCombinedCodeList(string codeListTableName)
        {
            if (string.IsNullOrWhiteSpace(codeListTableName))
                return BadRequest("codeListTableName cannot be empty");

            var result = await _service.GetCombinedCodeListAsync(codeListTableName);
            return Ok(result);
        }

        [HttpGet("values/by-parent/{shortStringValue}")]
        public async Task<ActionResult<List<CodeListValueDto>>> GetValuesByParentShortStringValue(string shortStringValue)
        {
            if (string.IsNullOrWhiteSpace(shortStringValue))
                return BadRequest("shortStringValue cannot be empty");

            var result = await _service.GetCodeListValuesByParentShortStringValueAsync(shortStringValue);
            return Ok(result);
        }

        [HttpPost("values")]
        public async Task<ActionResult<CodeListValueDto>> CreateCodeListValue([FromBody] CreateCodeListValueDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                var result = await _service.CreateCodeListValueAsync(dto);
                return CreatedAtAction(nameof(GetValuesByParentShortStringValue), new { shortStringValue = dto.ParentShortStringValue }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
