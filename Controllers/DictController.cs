using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Implementations;
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
        private readonly DictConfigManager _dictConfigManager;

        // 构造函数注入：业务服务 + 下拉框服务 + 字典配置管理器
        public DictController(
            IDictService dictService,
            IS3dCommonCodeListValueService codeListService,
            DictConfigManager dictConfigManager)
        {
            _dictService = dictService;
            _codeListService = codeListService;
            _dictConfigManager = dictConfigManager;
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
                // A. 【查配置】通过 DictConfigManager 根据 type 获取元数据
                //    统一从 Configs/DictConfigs/*.json 中解析，而不是直接访问 IConfiguration
                PMCSystem_Backend.Dtos.Dict.DictItemConfig config;
                try
                {
                    config = _dictConfigManager.GetConfig(type);
                }
                catch (Exception ex)
                {
                    // 未找到对应配置时返回 404
                    return NotFound(new { message = ex.Message });
                }

                // B. 校验是否配置了 CodeListTableName
                if (string.IsNullOrWhiteSpace(config.CodeListTableName))
                {
                    // 该类型不依赖 CodeList，视为不支持 options 接口
                    return NotFound(new
                    {
                        message = $"业务类型 '{type}' 未配置 CodeListTableName 或不支持下拉选项，请检查字典配置（Configs/DictConfigs 下相关 json）。"
                    });
                }

                // C. 【查数据】调用通用服务获取下拉选项
                var result = await _codeListService.GetOptionsAsync(config.CodeListTableName);

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

                // 调用 Service
                int newId = await _dictService.AddAsync(type, data);

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
                int affected = await _dictService.UpdateAsync(type, id, data);

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
        // =================================================================
        // 6. 批量删除 (Batch Delete)
        // URL: POST /api/dict/std-series/batch-delete
        // Payload: [1, 2, 3]
        // =================================================================


    }
}