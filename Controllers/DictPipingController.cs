using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Services.Implementations;
using System.Data;


namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictPipingController(IDictPipingService dictPipingService, 
                                    IS3dCommonCodeListValueService codeListService, 
                                    DictConfigManager dictConfigManager) : ControllerBase
    {
        private readonly IDictPipingService _dictPipingService = dictPipingService;
        private readonly IS3dCommonCodeListValueService _codeListService = codeListService;
        private readonly DictConfigManager _dictConfigManager = dictConfigManager;

        // =================================================================
        // 1. 下拉框选项接口 (Get Options)
        // URL: GET /api/dict/piping/options/std-series
        // =================================================================
        [HttpGet("options/{type}")]
        public async Task<IActionResult> GetOptions(string type)
        {
            try
            {
                // 使用 DictConfigManager 获取配置
                var config = _dictConfigManager.GetConfig(type);
                var codeListTableName = config.CodeListTableName;

                if (string.IsNullOrEmpty(codeListTableName))
                {
                    return NotFound(new { message = $"未找到业务类型 '{type}' 的 CodeListTableName 配置，请检查配置文件" });
                }

                // 调用通用服务获取下拉选项
                var result = await _codeListService.GetOptionsAsync(codeListTableName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"获取选项失败: {ex.Message}" });
            }
        }

        // =================================================================
        // 2. 查询列表 (Get List)
        // URL: GET /api/dict/std-series?keyword=xxx
        // =================================================================
        [HttpGet("{type}")]
        public async Task<IActionResult> GetTable(string type, [FromQuery] string? keyword = null)
        {
            try
            {
                // 获取表格数据 (支持分页和搜索，逻辑在 Service 里)
                var result = await _dictPipingService.GetTableDataAsync(type, keyword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // =================================================================
        // 3. 新增 (Create)
        // URL: POST /api/dict/std-series
        // Payload: { "GeometricIndustryPractice_CL": 10001, "Status": 1, ... }
        // =================================================================
        [HttpPost("{type}")]
        public async Task<IActionResult> Add(string type, [FromBody] DictInputDto data)
        {
            try
            {
                if (data == null || data.Count == 0)
                    return BadRequest(new { message = "提交数据不能为空" });

                // 调用 Service
                int newId = await _dictPipingService.AddAsync(type, data);

                return Ok(new { message = "新增成功", id = newId });
            }
            catch (Exception ex)
            {
                // 记录日志...
                return BadRequest(new { message = $"新增失败: {ex.Message}" });
            }
        }

        // =================================================================
        // 4. 修改 (Update)
        // URL: PUT /api/dict/std-series/5
        // =================================================================
        [HttpPut("{type}/{id}")]
        public async Task<IActionResult> Update(string type, int id, [FromBody] DictInputDto data)
        {
            try
            {
                if (data == null || data.Count == 0)
                    return BadRequest(new { message = "提交数据不能为空" });

                // 调用 Service 的 UpdateAsync
                int affected = await _dictPipingService.UpdateAsync(type, id, data);

                if (affected == 0)
                    return NotFound(new { message = "未找到记录或未做任何修改" });

                return Ok(new { message = "修改成功" });
            }
            catch (Exception ex)
            {
                // 建议记录 ex 日志
                return BadRequest(new { message = $"修改失败: {ex.Message}" });
            }
        }

        // =================================================================
        // 5. 删除 (Delete)
        // URL: DELETE /api/dict/std-series/5
        // =================================================================
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            try
            {
                int affected = await _dictPipingService.DeleteAsync(type, id);
                if (affected == 0)
                    return NotFound(new { message = "未找到记录或已被删除" });

                return Ok(new { message = "删除成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"删除失败: {ex.Message}" });
            }
        }
        // =================================================================
        // 6. 批量删除 (Batch Delete)
        // URL: POST /api/dict/std-series/batch-delete
        // Payload: [1, 2, 3]
        // =================================================================
        [HttpPost("{type}/batch-delete")]
        public async Task<IActionResult> BatchDelete(string type, [FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                    return BadRequest(new { message = "提交数据不能为空" });

                int affected = await _dictPipingService.BatchDeleteAsync(type, ids);
                if (affected == 0)
                    return NotFound(new { message = "未找到记录或已被删除" });

                return Ok(new { message = "批量删除成功", affected });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"批量删除失败: {ex.Message}" });
            }
        }

    }
}