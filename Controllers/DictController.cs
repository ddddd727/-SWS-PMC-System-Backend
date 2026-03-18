using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Implementations;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictController : ControllerBase
    {
        private readonly IDictService _dictService;
        private readonly DictConfigManager _configManager; // 移除了 _codeListService

        public DictController(
            IDictService dictService,
            DictConfigManager configManager)
        {
            _dictService = dictService;
            _configManager = configManager;
        }

        // ================================================================
        // 1. 下拉框选项
        // GET /api/dict/options/std-series
        // ================================================================
        [HttpGet("options/{type}")]
        [HttpGet("options/{type}")]
        public async Task<IActionResult> GetOptions(string type)
        {
            try
            {
                var config = _configManager.GetConfig(type);

                if (string.IsNullOrEmpty(config.CodeListTableName))
                    return NotFound(new { message = $"类型 '{type}' 未配置 CodeListTableName" });

                // 从该 type 的列配置里找到 LoadRelation（有配就带父子级，没配走原逻辑）
                var relation = config.Columns
                    .FirstOrDefault(c => c.DataSource?.LoadRelation != null)
                    ?.DataSource?.LoadRelation;

                // 【修改点】改为调用 _dictService.GetCodeListOptionsAsync
                var result = await _dictService.GetCodeListOptionsAsync(config.CodeListTableName, relation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"获取选项失败: {ex.Message}" });
            }
        }

        // ================================================================
        // 2. 查询列表
        // GET /api/dict/std-series?keyword=xxx
        // ================================================================
        [HttpGet("{type}")]
        public async Task<IActionResult> GetTable(string type, [FromQuery] string? keyword = null)
        {
            try
            {
                var result = await _dictService.GetTableDataAsync(type, keyword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ================================================================
        // 3. 新增
        // POST /api/dict/std-series
        // ================================================================
        [HttpPost("{type}")]
        public async Task<IActionResult> Add(string type, [FromBody] DictInputDto data)
        {
            try
            {
                if (data == null || data.Count == 0)
                    return BadRequest(new { message = "提交数据不能为空" });

                int newId = await _dictService.AddAsync(type, data);
                return Ok(new { message = "新增成功", id = newId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"新增失败: {ex.Message}" });
            }
        }

        // ================================================================
        // 4. 修改
        // PUT /api/dict/std-series/5
        // ================================================================
        [HttpPut("{type}/{id}")]
        public async Task<IActionResult> Update(string type, int id, [FromBody] DictInputDto data)
        {
            try
            {
                if (data == null || data.Count == 0)
                    return BadRequest(new { message = "提交数据不能为空" });

                int affected = await _dictService.UpdateAsync(type, id, data);
                if (affected == 0)
                    return NotFound(new { message = "未找到记录或未做任何修改" });

                return Ok(new { message = "修改成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"修改失败: {ex.Message}" });
            }
        }

        // ================================================================
        // 5. 删除
        // DELETE /api/dict/std-series/5
        // ================================================================
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            try
            {
                // Permissions 检查
                var config = _configManager.GetConfig(type);
                if (config.Permissions != null && !config.Permissions.AllowDelete)
                    return StatusCode(403, new { message = "该表格不允许删除操作" });

                int affected = await _dictService.DeleteAsync(type, id);
                if (affected == 0)
                    return NotFound(new { message = "未找到记录或已被删除" });

                return Ok(new { message = "删除成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"删除失败: {ex.Message}" });
            }
        }

        // ================================================================
        // 6. 远程字段校验（CustomRules.Type=Url 时前端调用）
        // POST /api/dict/validate/std-series
        // Body: { "field": "Code", "value": "ABC", "row": { ... } }
        // ================================================================
        [HttpPost("validate/{type}")]
        public async Task<IActionResult> ValidateField(string type, [FromBody] DictValidateRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Field))
                    return BadRequest(new { message = "请求参数不能为空" });

                var result = await _dictService.ValidateFieldAsync(type, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"校验失败: {ex.Message}" });
            }
        }
    }
}