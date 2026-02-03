using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Requests;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Controllers
{
    public class PmcSpecController : ApiControllerBase
    {
        private readonly IPmcSpecService _pmcSpecService;
        private readonly ILogger<PmcSpecController> _logger;

        /// <summary>
        /// 构造函数，注入所需服务
        /// </summary>
        public PmcSpecController(
            IPmcSpecService pmcSpecService,
            ILogger<PmcSpecController> logger)
        {
            _pmcSpecService = pmcSpecService;
            _logger = logger;
        }


        /// <summary>
        /// 获取所有船型船号信息
        /// </summary>
        /// <returns>船型船号信息</returns>
        [HttpGet("ShipInfos")]
        public IActionResult GetShipInfos()
        {
            try
            {
                _logger.LogInformation("开始获取船型船号信息");

                var shipInfos = _pmcSpecService.GetShipInfos();
                if (shipInfos == null || !shipInfos.Any())
                {
                    _logger.LogWarning("未找到船型船号信息");
                    return Fail(ApiErrorCode.ResourceNotFound, "未找到船型船号信息");
                }

                _logger.LogInformation("成功获取船型船号信息，共 {Count} 条", shipInfos.Count);
                return Success(shipInfos, "获取成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取船型船号信息时发生错误");
                return Fail(ApiErrorCode.BusinessRuleViolation, "获取船型船号信息失败，请稍后重试");
            }
        }

        /// <summary>
        /// 获取所有部件类型信息
        /// </summary>
        /// <returns>部件类型列表</returns>
        [HttpGet("ComponentTypes")]
        [ProducesResponseType(typeof(ApiResponse<List<ComponentTypeInfoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public IActionResult GetComponentTypes()
        {
            try
            {
                _logger.LogInformation("开始获取部件类型列表");

                var componentTypes = _pmcSpecService.GetComponentTypes();
                if (componentTypes == null || !componentTypes.Any())
                {
                    _logger.LogWarning("未找到部件类型信息");
                    return Fail(ApiErrorCode.ResourceNotFound, "未找到部件类型信息");
                }

                _logger.LogInformation("成功获取部件类型列表，共 {Count} 条", componentTypes.Count);
                return Success(componentTypes, "获取成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取部件类型列表时发生错误");
                return Fail(ApiErrorCode.BusinessRuleViolation, "获取部件类型列表失败，请稍后重试");
            }
        }

        /// <summary>
        /// 根据船号获取PMC编码信息
        /// </summary>
        /// <param name="shipNumber">船号</param>
        /// <returns>PMC编码列表信息</returns>
        /// <response code="200">查询成功，返回PMC编码列表</response>
        /// <response code="400">请求参数错误</response>
        /// <response code="404">未找到该船号对应的PMC编码数据</response>
        [HttpGet("PmcRules/{shipNumber}")]
        [ProducesResponseType(typeof(ApiResponse<List<PmcSelectInfoDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public IActionResult GetPmcRulesByShipNumber(
            [Required(ErrorMessage = "船号不能为空")] string shipNumber)
        {
            // 使用ModelState自动验证
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                _logger.LogInformation("开始获取船号 {ShipNumber} 的PMC编码信息", shipNumber);

                // 调用服务层方法查询PMC编码数据
                var pmcRules = _pmcSpecService.GetPmcRulesByShipNum(shipNumber);

                // 判断查询结果
                if (pmcRules == null || !pmcRules.Any())
                {
                    _logger.LogWarning("未找到船号 {ShipNumber} 对应的PMC编码数据", shipNumber);
                    return Fail(ApiErrorCode.ResourceNotFound, "未找到对应的PMC编码数据");
                }

                _logger.LogInformation("成功获取船号 {ShipNumber} 的PMC编码信息，共 {Count} 条", shipNumber, pmcRules.Count);
                return Success(pmcRules, "获取成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取船号 {ShipNumber} 的PMC编码信息时发生错误", shipNumber);
                return Fail(ApiErrorCode.BusinessRuleViolation, "获取PMC编码信息失败，请稍后重试");
            }
        }

        /// <summary>
        /// 解析PMC编码基础信息
        /// </summary>
        /// <param name="pmcCode">PMC 7位编码</param>
        /// <returns>PMC基础信息</returns>
        /// <response code="200">解析成功，返回PMC基础信息</response>
        /// <response code="400">请求参数错误</response>
        [HttpGet("Analyze/{pmcCode}")]
        [ProducesResponseType(typeof(ApiResponse<PmcBaseInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult AnalyzePmcCode(
            [Required(ErrorMessage = "PMC编码不能为空")] string pmcCode)
        {
            // 使用ModelState自动验证
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                _logger.LogInformation("开始解析PMC编码: {PmcCode}", pmcCode);

                var result = _pmcSpecService.AnalyzeCodeFromPMC(pmcCode);

                _logger.LogInformation("成功解析PMC编码: {PmcCode}", pmcCode);
                return Success(result, "解析成功");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "PMC编码格式错误: {PmcCode}", pmcCode);
                return Fail(ApiErrorCode.ValidationError, "PMC编码格式不正确，请检查编码是否为7位有效字符");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析PMC编码时发生错误: {PmcCode}", pmcCode);
                return Fail(ApiErrorCode.BusinessRuleViolation, "PMC编码解析失败，请检查编码是否正确");
            }
        }

        /// <summary>
        /// 根据端面标准和壁厚系列获取通径、外径、壁厚信息
        /// </summary>
        /// <param name="request">获取NPD信息请求</param>
        /// <returns>通径、外径、壁厚信息列表</returns>
        /// <response code="200">查询成功，返回通径、外径、壁厚信息</response>
        /// <response code="400">请求参数错误或查询失败</response>
        [HttpGet("NPDInfo")]
        [ProducesResponseType(typeof(ApiResponse<SpecNPDInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetNPDInfo([FromQuery] GetNPDInfoRequest request)
        {
            // 使用ModelState自动验证
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                _logger.LogInformation("开始获取NPD信息，端面标准: {EndStandard}, 壁厚系列: {Schedule}",
                    request.EndStandard, request.Schedule);

                // 调用服务层方法获取通径、外径、壁厚信息
                var result = _pmcSpecService.GetNPDInfoByPmc(request.EndStandard, request.Schedule);

                // 判断查询结果是否为空
                if (result == null || IsNPDInfoEmpty(result))
                {
                    _logger.LogWarning("未找到NPD信息，端面标准: {EndStandard}, 壁厚系列: {Schedule}",
                        request.EndStandard, request.Schedule);
                    return Fail(ApiErrorCode.ResourceNotFound, "未找到对应的NPD信息");
                }

                _logger.LogInformation("成功获取NPD信息，端面标准: {EndStandard}, 壁厚系列: {Schedule}",
                    request.EndStandard, request.Schedule);
                return Success(result, "获取成功");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "NPD信息参数验证失败，端面标准: {EndStandard}, 壁厚系列: {Schedule}",
                    request.EndStandard, request.Schedule);
                return Fail(ApiErrorCode.ValidationError, "参数验证失败");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取NPD信息时发生错误，端面标准: {EndStandard}, 壁厚系列: {Schedule}",
                    request.EndStandard, request.Schedule);
                return Fail(ApiErrorCode.BusinessRuleViolation, "查询失败，请稍后重试");
            }
        }

        /// <summary>
        /// 根据部件类型获取标准列表和对应的材料列表
        /// </summary>
        /// <param name="request">获取管附件规格请求</param>
        /// <returns>标准列表及每个标准对应的材料列表</returns>
        /// <response code="200">查询成功，返回标准列表和材料列表</response>
        /// <response code="400">请求参数错误或查询失败</response>
        /// <response code="404">未找到该部件类型对应的标准数据</response>
        [HttpGet("PipeFittingSpec")]
        [ProducesResponseType(typeof(ApiResponse<List<PipeFittingSpecDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public IActionResult GetPipeFittingSpec([FromQuery] GetPipeFittingSpecRequest request)
        {
            // 使用ModelState自动验证
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                _logger.LogInformation("开始获取管附件规格，部件类型: {ComponentTypeName}", request.ComponentTypeName);

                // 调用服务层方法获取标准列表和材料列表
                var result = _pmcSpecService.GetPipeFittingSpec(request.ComponentTypeName);

                // 判断查询结果是否为空
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("未找到管附件规格，部件类型: {ComponentTypeName}", request.ComponentTypeName);
                    return Fail(ApiErrorCode.ResourceNotFound, "未找到对应的标准列表和材料列表");
                }

                _logger.LogInformation("成功获取管附件规格，部件类型: {ComponentTypeName}，共 {Count} 条",
                    request.ComponentTypeName, result.Count);
                return Success(result, "获取成功");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "管附件规格参数验证失败，部件类型: {ComponentTypeName}", request.ComponentTypeName);
                return Fail(ApiErrorCode.ValidationError, "参数验证失败");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取管附件规格时发生错误，部件类型: {ComponentTypeName}", request.ComponentTypeName);
                return Fail(ApiErrorCode.BusinessRuleViolation, "查询失败，请稍后重试");
            }
        }

        /// <summary>
        /// 保存规格书配置信息
        /// </summary>
        /// <param name="request">保存规格书配置请求，包含船型、船号、PMC编码和部件类型配置列表</param>
        /// <returns>保存结果</returns>
        /// <response code="200">保存成功</response>
        /// <response code="400">请求参数错误或保存失败</response>
        [HttpPost("SpecRules")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult SaveSpecRules([FromBody] SavePipeSpecRequest request)
        {
            // 使用 ModelState 自动验证（基于 Data Annotations）
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                _logger.LogInformation("开始保存规格书配置，PMC编码: {PmcCode}, 船型: {ShipType}, 船号: {ShipNumber}",
                    request.PmcCode, request.ShipType, request.ShipNumber);

                // 调用服务层方法保存规格书配置
                var result = _pmcSpecService.SaveSpecRules(request);

                if (result)
                {
                    _logger.LogInformation("成功保存规格书配置，PMC编码: {PmcCode}", request.PmcCode);
                    return Success("规格书配置保存成功");
                }
                else
                {
                    _logger.LogWarning("规格书配置保存失败，PMC编码: {PmcCode}", request.PmcCode);
                    return Fail(ApiErrorCode.BusinessRuleViolation, "规格书配置保存失败");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "规格书配置参数验证失败，PMC编码: {PmcCode}", request.PmcCode);
                return Fail(ApiErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存规格书配置时发生错误，PMC编码: {PmcCode}", request.PmcCode);
                return Fail(ApiErrorCode.BusinessRuleViolation, "保存失败，请稍后重试");
            }
        }

        /// <summary>
        /// 检查NPD信息是否为空
        /// </summary>
        /// <param name="info">NPD信息对象</param>
        /// <returns>如果所有集合都为空则返回true</returns>
        private bool IsNPDInfoEmpty(SpecNPDInfoDto info)
        {
            return (info.NPD == null || !info.NPD.Any()) &&
                   (info.OutsideDiameter == null || !info.OutsideDiameter.Any()) &&
                   (info.WallThickness == null || !info.WallThickness.Any());
        }
    }
}
