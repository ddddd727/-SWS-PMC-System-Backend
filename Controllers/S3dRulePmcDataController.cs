using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    /// <summary>
    /// S3dRulePmcData 控制器
    /// </summary>
    public class S3dRulePmcDataController : ApiControllerBase
    {
        private readonly IS3dRulePmcDataService _service;

        public S3dRulePmcDataController(IS3dRulePmcDataService service)
        {
            _service = service;
        }

        /// <summary>
        /// 根据ID获取单条记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>S3dRulePmcData信息</returns>
        /// <response code="200">查询成功</response>
        /// <response code="404">记录不存在</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<S3dRulePmcDataDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return Fail(ApiErrorCode.ResourceNotFound, $"未找到ID为 {id} 的记录");
                }

                return Success(result, "查询成功");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"查询失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request">分页请求参数</param>
        /// <returns>分页结果</returns>
        /// <response code="200">查询成功</response>
        /// <response code="400">请求参数错误</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<S3dRulePmcDataDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequest request)
        {
            try
            {
                // 参数验证
                if (AutoValidate() is IActionResult validationResult)
                {
                    return validationResult;
                }

                var result = await _service.GetPagedAsync(request);
                return Paged(result, "查询成功");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"查询失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据PMC编码查询
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <returns>S3dRulePmcData列表</returns>
        /// <response code="200">查询成功</response>
        /// <response code="400">请求参数错误</response>
        [HttpGet("ByPmcCode/{pmcCode}")]
        [ProducesResponseType(typeof(ApiResponse<List<S3dRulePmcDataDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetByPmcCode(string pmcCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pmcCode))
                {
                    return Fail(ApiErrorCode.ValidationError, "PMC编码不能为空");
                }

                var result = await _service.GetByPmcCodeAsync(pmcCode);
                return Success(result, "查询成功");
            }
            catch (ArgumentException ex)
            {
                return Fail(ApiErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"查询失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据船号查询
        /// </summary>
        /// <param name="shipNo">船号</param>
        /// <returns>S3dRulePmcData列表</returns>
        /// <response code="200">查询成功</response>
        /// <response code="400">请求参数错误</response>
        [HttpGet("ByShipNo/{shipNo}")]
        [ProducesResponseType(typeof(ApiResponse<List<S3dRulePmcDataDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetByShipNo(string shipNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(shipNo))
                {
                    return Fail(ApiErrorCode.ValidationError, "船号不能为空");
                }

                var result = await _service.GetByShipNoAsync(shipNo);
                return Success(result, "查询成功");
            }
            catch (ArgumentException ex)
            {
                return Fail(ApiErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"查询失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建新记录
        /// </summary>
        /// <param name="dto">创建DTO</param>
        /// <returns>创建后的记录</returns>
        /// <response code="201">创建成功</response>
        /// <response code="400">请求参数错误或创建失败</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<S3dRulePmcDataDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Create([FromBody] CreateS3dRulePmcDataDto dto)
        {
            try
            {
                // 参数验证
                if (AutoValidate() is IActionResult validationResult)
                {
                    return validationResult;
                }

                if (dto == null)
                {
                    return Fail(ApiErrorCode.ValidationError, "请求参数不能为空");
                }

                var result = await _service.CreateAsync(dto);
                return Created(result, "创建成功");
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ApiErrorCode.DuplicateResource, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"创建失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="dto">更新DTO</param>
        /// <returns>更新结果</returns>
        /// <response code="200">更新成功</response>
        /// <response code="400">请求参数错误或更新失败</response>
        /// <response code="404">记录不存在</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateS3dRulePmcDataDto dto)
        {
            try
            {
                // 参数验证
                if (AutoValidate() is IActionResult validationResult)
                {
                    return validationResult;
                }

                if (dto == null)
                {
                    return Fail(ApiErrorCode.ValidationError, "请求参数不能为空");
                }

                var result = await _service.UpdateAsync(id, dto);

                if (!result)
                {
                    return Fail(ApiErrorCode.ResourceNotFound, $"未找到ID为 {id} 的记录");
                }

                return Success("更新成功");
            }
            catch (InvalidOperationException ex)
            {
                return Fail(ApiErrorCode.DuplicateResource, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"更新失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
        /// <response code="200">删除成功</response>
        /// <response code="404">记录不存在</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                if (!result)
                {
                    return Fail(ApiErrorCode.ResourceNotFound, $"未找到ID为 {id} 的记录");
                }

                return Success("删除成功");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">记录ID列表</param>
        /// <returns>删除结果</returns>
        /// <response code="200">删除成功</response>
        /// <response code="400">请求参数错误</response>
        [HttpPost("BatchDelete")]
        [ProducesResponseType(typeof(ApiResponse<int>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> BatchDelete([FromBody] List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return Fail(ApiErrorCode.ValidationError, "ID列表不能为空");
                }

                var deletedCount = await _service.DeleteBatchAsync(ids);
                return Success(deletedCount, $"成功删除 {deletedCount} 条记录");
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"批量删除失败: {ex.Message}");
            }
        }
    }
}
