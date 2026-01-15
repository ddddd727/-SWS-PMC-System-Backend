using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/pmc/pmccode")]
    public class PmcCodeController : ApiControllerBase
    {
        private readonly IPmcCodeService _service;
        private readonly ILogger<PmcCodeController> _logger;

        public PmcCodeController(IPmcCodeService service, ILogger<PmcCodeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("generate")]
        public IActionResult Generate([FromBody] PmcCodeGenerateRequest request)
        {
            try
            {
                if (request == null || request.PmcCodes == null || request.PmcCodes.Count == 0)
                {
                    return Success(new List<PmcCodeGenerateResponseItem>(), "PMC编码为空");
                }

                _logger.LogInformation("Received request to generate {Count} PMC codes", request.PmcCodes.Count);
                var data = _service.GenerateWithDescriptions(request.PmcCodes);
                return Success(data, "生成PMC编码描述成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate PMC codes");
                return StatusCode(500, new { code = 500, message = "生成PMC编码失败: " + ex.Message });
            }
        }

        [HttpPost("save")]
        public IActionResult Save([FromBody] PmcCodeSaveRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.ShipType) || string.IsNullOrWhiteSpace(request.ShipNo))
                {
                    return Fail(ApiErrorCode.ValidationError, "请填写船型船号");
                }

                _logger.LogInformation("Saving PMC codes for ShipType={ShipType}, ShipNo={ShipNo}", request.ShipType, request.ShipNo);
                _service.SavePmcCodes(request);
                return Success("PMC编码保存成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save PMC codes");
                return StatusCode(500, new { code = 500, message = "保存PMC编码失败: " + ex.Message });
            }
        }

        [HttpGet("query")]
        public IActionResult Query(string shipType, string shipNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(shipType) || string.IsNullOrWhiteSpace(shipNo))
                    return Fail(ApiErrorCode.ValidationError, "请填写船型船号");

                var data = _service.GetPmcCodes(shipType, shipNo);
                return Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to query PMC codes");
                return StatusCode(500, new { code = 500, message = "查询PMC编码失败: " + ex.Message });
            }
        }
    }
}
