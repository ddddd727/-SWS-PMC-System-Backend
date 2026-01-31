using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // ✅ 核心：用于读取 JSON 配置
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/[controller]")] // 路由前缀: /api/dict
    [ApiController]
    public class DictController : ControllerBase
    {
        private readonly IDictService _dictService;
        private readonly IS3dCommonCodeListValueService _codeListService;
        private readonly IConfiguration _configuration;

        // 构造函数注入：业务服务 + 下拉框服务 + 配置读取器
        public DictController(
            IDictService dictService,
            IS3dCommonCodeListValueService codeListService,
            IConfiguration configuration)
        {
            _dictService = dictService;
            _codeListService = codeListService;
            _configuration = configuration;
        }

        // =================================================================
        // 1. 下拉框选项接口 (Get Options)
        // URL: GET /api/dict/options/std-series
        // =================================================================
        [HttpGet("options/{type}")]
        public async Task<IActionResult> GetOptions(string type)
        {
            try
            {
                // A. 【查配置】根据业务代号 (std-series) 获取真实 CodeList 表名
                // 路径对应 dicts.json: DictConfiguration -> std-series -> CodeListTableName
                var codeListTableName = _configuration[$"DictConfiguration:{type}:CodeListTableName"];

                if (string.IsNullOrEmpty(codeListTableName))
                {
                    return NotFound(new { message = $"未找到业务类型 '{type}' 的 CodeListTableName 配置，请检查 dicts.json" });
                }

                // B. 【查数据】调用通用服务获取下拉选项
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
                var result = await _dictService.GetTableDataAsync(type, keyword);
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

                await _dictService.AddAsync(type, data);
                return Ok(new { message = "添加成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"添加失败: {ex.Message}" });
            }
        }

        // =================================================================
        // 4. 修改 (Update)
        // URL: PUT /api/dict/std-series/5
        // =================================================================
       

        // =================================================================
        // 5. 删除 (Delete)
        // URL: DELETE /api/dict/std-series/5
        // =================================================================
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            try
            {
                await _dictService.DeleteAsync(type, id);
                return Ok(new { message = "删除成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"删除失败: {ex.Message}" });
            }
        }

        // =================================================================
        // 6. 批量删除 (Batch Delete)
        // URL: POST /api/dict/std-series/batch-delete
        // Payload: [1, 2, 3]
        // =================================================================
        
        
    }
}