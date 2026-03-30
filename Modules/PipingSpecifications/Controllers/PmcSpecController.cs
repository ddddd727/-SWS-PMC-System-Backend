using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.Requests;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared;
using PMCSystem_Backend.Shared.Enums;
using PMCSystem_Backend.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Controllers
{
    public class PmcSpecController : ApiControllerBase
    {
        private readonly IPmcSpecService _pmcSpecService;
        private readonly IPipeSpecVersionService _pipeSpecVersionService;
        private readonly ILogger<PmcSpecController> _logger;

        /// <summary>
        /// 构造函数，注入所需服务
        /// </summary>
        public PmcSpecController(
            IPmcSpecService pmcSpecService,
            IPipeSpecVersionService pipeSpecVersionService,
            ILogger<PmcSpecController> logger)
        {
            _pmcSpecService = pmcSpecService;
            _pipeSpecVersionService = pipeSpecVersionService;
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
        /// 解析PMC编码基础信息（包含配置信息）
        /// </summary>
        /// <param name="pmcCode">PMC 7位编码</param>
        /// <returns>PMC基础信息和配置信息（如果已配置）</returns>
        /// <response code="200">解析成功，返回PMC基础信息和配置信息</response>
        /// <response code="400">请求参数错误</response>
        [HttpGet("Analyze/{pmcCode}")]
        [ProducesResponseType(typeof(ApiResponse<PmcInfoWithConfigDto>), 200)]
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

                // 调用新方法，返回基础信息和配置信息
                var result = _pmcSpecService.AnalyzeCodeFromPMCWithConfig(pmcCode);

                _logger.LogInformation("成功解析PMC编码: {PmcCode}，是否已配置: {IsConfigured}", pmcCode, result.IsConfigured);
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
        public async Task<IActionResult> GetNPDInfo([FromQuery] GetNPDInfoRequest request)
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
                var result = await _pmcSpecService.GetNPDInfoByPmcAsync(request.EndStandard, request.Schedule);

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
        /// 根据部件类型获取管附件标准名称列表。
        /// </summary>
        /// <param name="request">获取管附件规格请求</param>
        /// <returns>标准名称列表</returns>
        /// <response code="200">查询成功，返回标准名称列表</response>
        /// <response code="400">请求参数错误或查询失败</response>
        /// <response code="404">未找到该部件类型对应的标准数据</response>
        [HttpGet("PipeFittingSpec")]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), 200)]
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
                _logger.LogInformation(
                    "开始获取管附件标准，部件类型: Id={ComponentTypeId}, Name={ComponentTypeName}",
                    request.ComponentTypeId,
                    request.ComponentTypeName);

                var result = _pmcSpecService.GetPipeFittingSpec(request.ComponentTypeId, request.ComponentTypeName);

                if (result == null || !result.Any())
                {
                    _logger.LogWarning(
                        "未找到管附件标准，部件类型: Id={ComponentTypeId}, Name={ComponentTypeName}",
                        request.ComponentTypeId,
                        request.ComponentTypeName);
                    return Fail(ApiErrorCode.ResourceNotFound, "未找到对应的标准列表");
                }

                _logger.LogInformation(
                    "成功获取管附件标准，部件类型: Id={ComponentTypeId}, Name={ComponentTypeName}，共 {Count} 条",
                    request.ComponentTypeId,
                    request.ComponentTypeName,
                    result.Count);
                return Success(result, "获取成功");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    ex,
                    "管附件标准参数验证失败，部件类型: Id={ComponentTypeId}, Name={ComponentTypeName}",
                    request.ComponentTypeId,
                    request.ComponentTypeName);
                return Fail(ApiErrorCode.ValidationError, "参数验证失败");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "获取管附件标准时发生错误，部件类型: Id={ComponentTypeId}, Name={ComponentTypeName}",
                    request.ComponentTypeId,
                    request.ComponentTypeName);
                return Fail(ApiErrorCode.BusinessRuleViolation, "查询失败，请稍后重试");
            }
        }

        /// <summary>
        /// 获取所有材料牌号列表（来自视图 S3D_CL_MaterialsGrade，仅返回 ShortStringValue 列）。
        /// </summary>
        /// <returns>材料牌号列表</returns>
        /// <response code="200">查询成功，返回材料牌号列表</response>
        /// <response code="400">查询失败</response>
        [HttpGet("MaterialsGrades")]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetMaterialsGrades()
        {
            try
            {
                _logger.LogInformation("开始获取材料牌号列表");

                var result = _pmcSpecService.GetMaterialsGrades();

                _logger.LogInformation("成功获取材料牌号列表，共 {Count} 条", result.Count);
                return Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取材料牌号列表时发生错误");
                return Fail(ApiErrorCode.BusinessRuleViolation, "查询失败，请稍后重试");
            }
        }

        /// <summary>
        /// 保存规格书配置信息（简化版：仅包含标准名称和材料信息，不包含通径范围）
        /// </summary>
        /// <param name="request">简化的保存规格书配置请求，包含船型、船号、PMC编码和部件类型配置列表</param>
        /// <returns>保存结果</returns>
        /// <response code="200">保存成功</response>
        /// <response code="400">请求参数错误或保存失败</response>
        [HttpPost("SpecRules")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult SaveSpecRules([FromBody] SavePipeSpecSimpleRequest request)
        {
            // 使用 ModelState 自动验证（基于 Data Annotations）
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                _logger.LogInformation("开始保存简化规格书配置，PMC编码: {PmcCode}, 船型: {ShipType}, 船号: {ShipNumber}",
                    request.PmcCode, request.ShipType, request.ShipNumber);

                // 调用服务层方法保存简化规格书配置
                var result = _pmcSpecService.SaveSpecRulesSimple(request);

                if (result)
                {
                    _logger.LogInformation("成功保存简化规格书配置，PMC编码: {PmcCode}", request.PmcCode);
                    return Success("规格书配置保存成功");
                }
                else
                {
                    _logger.LogWarning("简化规格书配置保存失败，PMC编码: {PmcCode}", request.PmcCode);
                    return Fail(ApiErrorCode.BusinessRuleViolation, "规格书配置保存失败");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "简化规格书配置参数验证失败，PMC编码: {PmcCode}", request.PmcCode);
                return Fail(ApiErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存简化规格书配置时发生错误，PMC编码: {PmcCode}", request.PmcCode);
                return Fail(ApiErrorCode.BusinessRuleViolation, "保存失败，请稍后重试");
            }
        }

        /// <summary>
        /// 接受规格书审核（占位，默认审核成功，后续接入审核系统）
        /// </summary>
        /// <param name="request">包含 PMC 编码，可选船型、船号</param>
        /// <returns>是否更新成功</returns>
        /// <response code="200">审核接受成功</response>
        /// <response code="400">参数错误或未找到对应记录</response>
        [HttpPost("AcceptReview")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult AcceptSpecReview([FromBody] AcceptSpecReviewRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.PmcCode))
            {
                return Fail(ApiErrorCode.ValidationError, "PMC编码不能为空");
            }

            try
            {
                var result = _pmcSpecService.AcceptSpecReview(
                    request.PmcCode,
                    string.IsNullOrWhiteSpace(request.ShipType) ? null : request.ShipType,
                    string.IsNullOrWhiteSpace(request.ShipNumber) ? null : request.ShipNumber);

                if (result)
                {
                    _logger.LogInformation("接受规格书审核成功，PMC编码: {PmcCode}", request.PmcCode);
                    return Success("审核已通过");
                }

                return Fail(ApiErrorCode.ResourceNotFound, "未找到对应的规格书配置记录");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "接受规格书审核时发生错误，PMC编码: {PmcCode}", request.PmcCode);
                return Fail(ApiErrorCode.BusinessRuleViolation, "操作失败，请稍后重试");
            }
        }

        /// <summary>
        /// 获取管系规格书历史版本列表（分页）
        /// </summary>
        /// <param name="pmcCode">PMC 编码</param>
        /// <param name="shipType">船型（可选，与 shipNumber 同时提供时精确匹配）</param>
        /// <param name="shipNumber">船号（可选）</param>
        /// <param name="pageIndex">页码，从 1 开始，默认 1</param>
        /// <param name="pageSize">每页条数，默认 20</param>
        /// <returns>版本列表及总数</returns>
        /// <response code="200">查询成功</response>
        /// <response code="400">参数错误</response>
        [HttpGet("{pmcCode}/versions")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<PipeSpecVersionDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetVersionList(
            [Required(ErrorMessage = "PMC编码不能为空")] string pmcCode,
            [FromQuery] string? shipType = null,
            [FromQuery] string? shipNumber = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20)
        {
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                var (items, totalCount) = _pipeSpecVersionService.GetVersionList(
                    pmcCode, shipType, shipNumber, pageIndex, pageSize);

                var result = new PagedResult<PipeSpecVersionDto>
                {
                    Items = items,
                    TotalCount = totalCount
                };
                _logger.LogInformation("成功获取 PMC {PmcCode} 版本列表，共 {Total} 条", pmcCode, totalCount);
                return Paged(result, "查询成功");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "版本列表参数验证失败，PMC编码: {PmcCode}", pmcCode);
                return Fail(ApiErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取版本列表时发生错误，PMC编码: {PmcCode}", pmcCode);
                return Fail(ApiErrorCode.BusinessRuleViolation, "查询失败，请稍后重试");
            }
        }

        /// <summary>
        /// 获取管系规格书历史版本详情
        /// </summary>
        /// <param name="pmcCode">PMC 编码</param>
        /// <param name="versionId">版本记录主键 Id</param>
        /// <returns>版本详情（含完整配置）</returns>
        /// <response code="200">查询成功</response>
        /// <response code="404">版本不存在</response>
        [HttpGet("{pmcCode}/versions/{versionId:int}")]
        [ProducesResponseType(typeof(ApiResponse<PipeSpecVersionDetailDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public IActionResult GetVersionDetail(
            [Required(ErrorMessage = "PMC编码不能为空")] string pmcCode,
            [Required(ErrorMessage = "版本ID不能为空")] int versionId)
        {
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                var result = _pipeSpecVersionService.GetVersionDetail(versionId);

                if (result == null)
                {
                    _logger.LogWarning("未找到版本 Id {VersionId}", versionId);
                    return Fail(ApiErrorCode.ResourceNotFound, "版本不存在");
                }

                if (result.BaseInfo.PmcCode != pmcCode)
                {
                    _logger.LogWarning("版本 {VersionId} 与 PMC 编码 {PmcCode} 不匹配", versionId, pmcCode);
                    return Fail(ApiErrorCode.ResourceNotFound, "版本与 PMC 编码不匹配");
                }

                _logger.LogInformation("成功获取版本 {VersionId} 详情", versionId);
                return Success(result, "查询成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取版本详情时发生错误，版本Id: {VersionId}", versionId);
                return Fail(ApiErrorCode.BusinessRuleViolation, "查询失败，请稍后重试");
            }
        }

        /// <summary>
        /// 使用历史版本覆盖当前版本
        /// </summary>
        /// <param name="pmcCode">PMC 编码</param>
        /// <param name="versionId">版本记录主键 Id</param>
        /// <param name="request">可选船型、船号用于精确匹配主表记录</param>
        /// <returns>是否成功</returns>
        /// <response code="200">回滚成功</response>
        /// <response code="404">版本或主表记录不存在</response>
        [HttpPost("{pmcCode}/versions/{versionId:int}/revert")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public IActionResult RevertToVersion(
            [Required(ErrorMessage = "PMC编码不能为空")] string pmcCode,
            [Required(ErrorMessage = "版本ID不能为空")] int versionId,
            [FromBody] RevertToVersionRequest? request = null)
        {
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }

            try
            {
                var shipType = request?.ShipType;
                var shipNumber = request?.ShipNumber;

                var result = _pipeSpecVersionService.RevertToVersion(versionId, shipType, shipNumber);

                if (result)
                {
                    _logger.LogInformation("成功将 PMC {PmcCode} 回滚至版本 {VersionId}", pmcCode, versionId);
                    return Success("已使用历史版本覆盖当前配置");
                }

                _logger.LogWarning("回滚失败，版本 {VersionId} 或主表记录不存在", versionId);
                return Fail(ApiErrorCode.ResourceNotFound, "版本或主表记录不存在，无法回滚");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回滚至版本时发生错误，版本Id: {VersionId}", versionId);
                return Fail(ApiErrorCode.BusinessRuleViolation, "回滚失败，请稍后重试");
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
