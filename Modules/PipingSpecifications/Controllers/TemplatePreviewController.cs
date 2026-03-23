using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared;
using PMCSystem_Backend.Shared.Enums;
using PMCSystem_Backend.Shared.Models;
using System.Security.Cryptography;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Controllers
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
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// 构造函数，注入模板预览服务与日志
        /// </summary>
        public TemplatePreviewController(
            ITemplatePreviewService templatePreviewService,
            ILogger<TemplatePreviewController> logger,
            IWebHostEnvironment environment)
        {
            _templatePreviewService = templatePreviewService;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// 获取指定模板的预览数据（RESTful: GET 资源）。预览需包含用户表单数据，故 pmcCode 必填，按已保存规格书填充模板占位符。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="pmcCode">必填。PMC 编码，用于获取已保存的规格书数据并填充模板</param>
        /// <returns>包装后的预览数据（含已填充的业务表单数据）</returns>
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
            [FromQuery] string pmcCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pmcCode))
                {
                    return Fail(ApiErrorCode.InvalidParameter, "PmcCode is required for template preview");
                }
                var preview = _templatePreviewService.GetTemplatePreviewBySpec(templateId, pmcCode.Trim());
                return Success(preview);
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
        /// 导出用户确认完的预览表格结果为 Excel 文件（RESTful: GET 子资源 /export）。使用与预览相同的 pmcCode 生成填充后的表格。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="pmcCode">必填。PMC 编码，用于获取已保存的规格书数据，与预览时传入的 pmcCode 一致</param>
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
            [FromQuery] string pmcCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pmcCode))
                {
                    return Fail(ApiErrorCode.InvalidParameter, "PmcCode is required for template export");
                }
                var fileContent = _templatePreviewService.ExportTemplateBySpec(templateId, pmcCode.Trim());
                var sig = fileContent.Length >= 4
                    ? BitConverter.ToString(fileContent, 0, 4)
                    : "N/A";
                var sha256 = Convert.ToHexString(SHA256.HashData(fileContent));
                Response.Headers["X-Export-Sha256"] = sha256;
                _logger.LogInformation("模板导出响应，TemplateId: {TemplateId}, PmcCode: {PmcCode}, Size: {Size}, Signature: {Signature}, Sha256: {Sha256}",
                    templateId, pmcCode, fileContent.Length, sig, sha256);

                if (_environment.IsDevelopment())
                {
                    var debugDir = Path.Combine(Path.GetTempPath(), "PMCSystem_Backend", "ExportDebug");
                    Directory.CreateDirectory(debugDir);
                    var debugPath = Path.Combine(debugDir, $"{templateId}_{DateTime.Now:yyyyMMddHHmmssfff}.xlsx");
                    System.IO.File.WriteAllBytes(debugPath, fileContent);
                    _logger.LogInformation("已写入导出调试文件，Path: {Path}", debugPath);
                }
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
