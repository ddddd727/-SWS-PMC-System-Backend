using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.CodeListManagement;
using PMCSystem_Backend.Services.Interfaces.CodeListManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers.CodeListManagement
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

        [HttpGet("hierarchy/{codeListTableName}")]
        public async Task<IActionResult> GetHierarchy(string codeListTableName)
        {
            if (string.IsNullOrWhiteSpace(codeListTableName))
                return BadRequest("codeListTableName cannot be empty");

            var hierarchy = await _service.GetHierarchyNamesAsync(codeListTableName);
            if (hierarchy.Count == 0)
                return NotFound();

            return Ok(hierarchy);
        }

        [HttpGet("multilevel")]
        public async Task<ActionResult<MultiLevelCodeListResponseDto>> GetMultiLevelValues(string? level1 = null, string? level2 = null, string? level3 = null, string? level4 = null, string? level5 = null)
        {
            var request = new MultiLevelCodeListRequestDto
            {
                Level1 = level1,
                Level2 = level2,
                Level3 = level3,
                Level4 = level4,
                Level5 = level5
            };

            var result = await _service.GetMultiLevelCodeListValuesAsync(request);
            return Ok(result);
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
    }
}
