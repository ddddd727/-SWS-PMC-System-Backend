using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    /// <summary>
    /// 模板预览与导出 API
    /// </summary>
    /// <remarks>
    /// 提供按模板 ID 获取预览数据及导出 Excel 文件的能力，遵循 RESTful 资源与子资源设计。
    /// </remarks>
    [ApiController]
    [Route("api/template-preview")]
    [Produces("application/json")]
    public class TemplatePreviewController : ApiControllerBase
    {
        private readonly ILogger<TemplatePreviewController> _logger;
        private readonly ITemplatePreviewService _templatePreviewService;

        /// <summary>
        /// 构造函数，注入模板预览服务与日志
        /// </summary>
        public TemplatePreviewController(
            ITemplatePreviewService templatePreviewService,
            ILogger<TemplatePreviewController> logger)
        {
            _templatePreviewService = templatePreviewService;
            _logger = logger;
        }

        /// <summary>
        /// 获取指定模板的预览数据（RESTful: GET 资源）。优先使用规格书数据：传入 pmcCode 时按已保存规格书填充；否则使用 parameters 键值对替换占位符。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="pmcCode">可选。PMC 编码；传入时使用已保存的规格书数据填充模板中的 "{{xxx}}" 占位符</param>
        /// <param name="parameters">可选。当未传 pmcCode 时，用于替换模板占位符的键值对</param>
        /// <returns>包装后的预览数据</returns>
        /// <response code="200">成功返回预览数据</response>
        /// <response code="400">请求参数错误或系统异常</response>
        /// <response code="404">模板不存在</response>
        [HttpGet("{templateId}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult GetPreview(
            [FromRoute] string templateId,
            [FromQuery] string? pmcCode,
            [FromQuery] Dictionary<string, string>? parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(pmcCode))
                {
                    var preview = _templatePreviewService.GetTemplatePreviewBySpec(templateId, pmcCode.Trim());
                    return Success(preview);
                }
                var previewCustom = _templatePreviewService.GetTemplatePreview(templateId, parameters ?? new Dictionary<string, string>());
                return Success(previewCustom);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for templateId: {TemplateId}, pmcCode: {PmcCode}", templateId, pmcCode);
                return Fail(ApiErrorCode.InvalidParameter, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting template preview for templateId: {TemplateId}", templateId);
                return ReturnSystemError("Internal server error");
            }
        }

        /// <summary>
        /// 导出指定模板为 Excel 文件（RESTful: GET 子资源 /export）。传入 pmcCode 时使用已保存规格书数据填充；否则使用 parameters 替换占位符。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="pmcCode">可选。PMC 编码；传入时使用已保存的规格书数据填充模板</param>
        /// <param name="parameters">可选。当未传 pmcCode 时，用于替换模板占位符的键值对</param>
        /// <returns>Excel 文件流，或错误响应</returns>
        /// <response code="200">成功返回 Excel 文件</response>
        /// <response code="400">参数无效</response>
        /// <response code="404">模板文件不存在</response>
        /// <response code="500">服务器内部错误</response>
        [HttpGet("{templateId}/export")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/json")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Export(
            [FromRoute] string templateId,
            [FromQuery] string? pmcCode,
            [FromQuery] Dictionary<string, string>? parameters)
        {
            try
            {
                byte[] fileContent;
                if (!string.IsNullOrWhiteSpace(pmcCode))
                    fileContent = _templatePreviewService.ExportTemplateBySpec(templateId, pmcCode.Trim());
                else
                    fileContent = _templatePreviewService.ExportTemplate(templateId, parameters ?? new Dictionary<string, string>());
                var fileName = $"{templateId}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(
                    fileContent,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Template file not found for templateId: {TemplateId}", templateId);
                return Fail(ApiErrorCode.ResourceNotFound, "Template file not found");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid templateId or pmcCode: {TemplateId}, {PmcCode}", templateId, pmcCode);
                return Fail(ApiErrorCode.InvalidParameter, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting template for templateId: {TemplateId}", templateId);
                return ReturnSystemError("Internal server error");
            }
        }

        /// <summary>
        /// 返回 500 系统错误响应（用于与基类 Fail 的 400 区分）
        /// </summary>
        private IActionResult ReturnSystemError(string message)
        {
            var response = ApiResponse.Fail((int)ApiErrorCode.SystemError, message);
            response.TraceId = TraceId;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}
