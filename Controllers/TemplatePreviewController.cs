using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/template-preview")]
    public class TemplatePreviewController : ApiControllerBase
    {
        private readonly ILogger<TemplatePreviewController> _logger;
        private readonly ITemplatePreviewService _templatePreviewService;
        public TemplatePreviewController(ITemplatePreviewService templatePreviewService, ILogger<TemplatePreviewController> logger)
        {
            _templatePreviewService = templatePreviewService;
            _logger = logger;
        }

        [HttpGet("{templateId}")]
        public IActionResult GetPreview([FromRoute] string templateId, [FromQuery] Dictionary<string, string>? parameters)
        {
            try
            {
                var preview = _templatePreviewService.GetTemplatePreview(templateId, parameters);
                return Success(preview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting template preview for templateId: {TemplateId}", templateId);
                return Fail(ApiErrorCode.SystemError, "Internal server error");
            }
        }

        [HttpPost("export/{templateId}")]
        public IActionResult Export([FromRoute] string templateId, [FromQuery] Dictionary<string, string>? parameters)
        {
            try
            {
                var fileContent = _templatePreviewService.ExportTemplate(templateId, parameters);
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{templateId}_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Template file not found for templateId: {TemplateId}", templateId);
                return Fail(ApiErrorCode.ResourceNotFound, "Template file not found");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid templateId format: {TemplateId}", templateId);
                return Fail(ApiErrorCode.InvalidParameter, "Invalid templateId format");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting template for templateId: {TemplateId}", templateId);
                return Fail(ApiErrorCode.SystemError, "Internal server error");
            }
        }
    }
}
