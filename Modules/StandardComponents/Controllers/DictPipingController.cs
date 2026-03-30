using System;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Services.Implementations;
using PMCSystem_Backend.Services.Interfaces;
using System.Data;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;


namespace PMCSystem_Backend.Modules.StandardComponents.Controllers
{
    /// <summary>
    /// 管道字典控制器
    /// 提供管道组件相关的CRUD操作
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DictPipingController(IDictPipingService dictPipingService,
                                    IDictService dictService,
                                    DictConfigManager dictConfigManager) : ControllerBase
    {
        private readonly IDictPipingService _dictPipingService = dictPipingService;
        private readonly IDictService _dictService = dictService;
        private readonly DictConfigManager _dictConfigManager = dictConfigManager;

        // =================================================================
        // 1. 下拉框选项接口 (Get Options)
        // URL: GET /api/dict/piping/options/std-series
        // =================================================================
        /// <summary>
        /// 获取下拉框选项
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <returns>下拉框选项列表</returns>
        /// <param name="field">可选，列 DbField。同一字典存在多个 Select 且策略不同时传入。</param>
        /// <param name="source">可选，覆盖选项策略：view / codelist。</param>
        [HttpGet("options/{type}")]
        public async Task<IActionResult> GetOptions(string type, [FromQuery] string? field = null, [FromQuery] string? source = null)
        {
            try
            {
                var result = await _dictService.GetDropdownOptionsAsync(type, field, source);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
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
        /// <summary>
        /// 查询管道组件列表
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns>组件列表数据</returns>
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
        /// <summary>
        /// 新增管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="data">组件数据</param>
        /// <returns>新增结果</returns>
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
        /// <summary>
        /// 更新管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">记录ID</param>
        /// <param name="data">更新的数据</param>
        /// <returns>更新结果</returns>
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
        /// <summary>
        /// 删除管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
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
        /// <summary>
        /// 批量删除管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="ids">记录ID列表</param>
        /// <returns>批量删除结果</returns>
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

        // =================================================================
        // 7. 获取ComponentType列表
        // URL: GET /api/dict/piping/componentType
        // =================================================================
        /// <summary>
        /// 获取管道组件类型列表
        /// </summary>
        /// <returns>组件类型列表</returns>
        [HttpGet("componentType")]
        public async Task<IActionResult> GetComponentTypeList()
        {
            try
            {
                var result = await _dictPipingService.GetComponentTypeListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"获取ComponentType列表失败: {ex.Message}" });
            }
        }

    }
}
