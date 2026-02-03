using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Requests;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Controllers
{
    public class PmcSpecController : ApiControllerBase
    {
        // 声明所需要的数据处理接口
        private readonly IPmcSpecService _pmcSpecService;

        // 通过构造函数注入所需要的数据处理接口
        public PmcSpecController(IPmcSpecService pmcSpecService)
        {
            _pmcSpecService = pmcSpecService;
        }


        /// <summary>
        /// 获取所有船型船号信息
        /// </summary>
        /// <returns>船型船号信息</returns>
        [HttpGet("ShipInfos")]
        public IActionResult GetShipInfos()
        {
            var shipInfos = _pmcSpecService.GetShipInfos();
            if (shipInfos == null || shipInfos.Count == 0)
            {
                return Fail(ApiErrorCode.ResourceNotFound, "获取船型船号信息失败");
            }
            return Success(shipInfos, "获取船型船号信息成功");
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
            var componentTypes = _pmcSpecService.GetComponentTypes();
            if (componentTypes == null || componentTypes.Count == 0)
            {
                return Fail(ApiErrorCode.ResourceNotFound, "获取部件类型列表失败");
            }
            return Success(componentTypes, "获取部件类型列表成功");
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
        public IActionResult GetPmcRulesByShipNumber(string shipNumber)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(shipNumber))
            {
                return Fail(ApiErrorCode.ValidationError, "船号不能为空");
            }

            // 调用服务层方法查询PMC编码数据
            var pmcRules = _pmcSpecService.GetPmcRulesByShipNum(shipNumber);

            // 判断查询结果
            if (pmcRules == null)
            {
                return Fail(ApiErrorCode.ResourceNotFound, $"未找到船号 {shipNumber} 对应的PMC编码数据");
            }

            if (pmcRules.Count == 0)
            {
                return Fail(ApiErrorCode.ResourceNotFound, $"船号 {shipNumber} 下暂无PMC编码数据");
            }
            return Success(pmcRules, $"成功获取船号 {shipNumber} 的PMC编码信息");
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
        public IActionResult AnalyzePmcCode(string pmcCode)
        {
            // 参数校验
            if (string.IsNullOrWhiteSpace(pmcCode))
            {
                return Fail(ApiErrorCode.ValidationError, "PMC编码不能为空");
            }

            try
            {
                var result = _pmcSpecService.AnalyzeCodeFromPMC(pmcCode);
                return Success(result, "PMC编码解析成功");
            }
            catch (Exception ex)
            {
                // 这里直接返回业务规则校验错误，可根据需要改成更细分的错误码
                return Fail(ApiErrorCode.BusinessRuleViolation, $"PMC编码解析失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据端面标准和壁厚系列获取通径、外径、壁厚信息
        /// </summary>
        /// <param name="endStandard">端面标准</param>
        /// <param name="schedule">壁厚系列</param>
        /// <returns>通径、外径、壁厚信息列表</returns>
        /// <response code="200">查询成功，返回通径、外径、壁厚信息</response>
        /// <response code="400">请求参数错误或查询失败</response>
        [HttpGet("NPDInfo")]
        [ProducesResponseType(typeof(ApiResponse<SpecNPDInfoDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetNPDInfo([FromQuery] string endStandard, [FromQuery] string schedule)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(endStandard))
            {
                return Fail(ApiErrorCode.ValidationError, "端面标准不能为空");
            }

            if (string.IsNullOrWhiteSpace(schedule))
            {
                return Fail(ApiErrorCode.ValidationError, "壁厚系列不能为空");
            }

            try
            {
                // 调用服务层方法获取通径、外径、壁厚信息
                var result = _pmcSpecService.GetNPDInfoByPmc(endStandard, schedule);

                // 判断查询结果是否为空
                if (result == null ||
                    (result.NPD == null || result.NPD.Count == 0) &&
                    (result.OutsideDiameter == null || result.OutsideDiameter.Count == 0) &&
                    (result.WallThickness == null || result.WallThickness.Count == 0))
                {
                    return Fail(ApiErrorCode.ResourceNotFound,
                        $"未找到端面标准 {endStandard} 和壁厚系列 {schedule} 对应的通径、外径、壁厚信息");
                }

                return Success(result, "成功获取通径、外径、壁厚信息");
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
        /// 根据部件类型获取标准列表和对应的材料列表
        /// </summary>
        /// <param name="componentTypeName">部件类型名称</param>
        /// <returns>标准列表及每个标准对应的材料列表</returns>
        /// <response code="200">查询成功，返回标准列表和材料列表</response>
        /// <response code="400">请求参数错误或查询失败</response>
        /// <response code="404">未找到该部件类型对应的标准数据</response>
        [HttpGet("PipeFittingSpec")]
        [ProducesResponseType(typeof(ApiResponse<List<PipeFittingSpecDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public IActionResult GetPipeFittingSpec([FromQuery] string componentTypeName)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(componentTypeName))
            {
                return Fail(ApiErrorCode.ValidationError, "部件类型名称不能为空");
            }

            try
            {
                // 调用服务层方法获取标准列表和材料列表
                var result = _pmcSpecService.GetPipeFittingSpec(componentTypeName);

                // 判断查询结果是否为空
                if (result == null || result.Count == 0)
                {
                    return Fail(ApiErrorCode.ResourceNotFound,
                        $"未找到部件类型 {componentTypeName} 对应的标准列表和材料列表");
                }

                return Success(result, $"成功获取部件类型 {componentTypeName} 的标准列表和材料列表");
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
                return Fail(ApiErrorCode.ValidationError, "请求参数验证失败");
            }

            try
            {
                // 调用服务层方法保存规格书配置
                var result = _pmcSpecService.SaveSpecRules(request);

                if (result)
                {
                    return Success("规格书配置保存成功");
                }
                else
                {
                    return Fail(ApiErrorCode.BusinessRuleViolation, "规格书配置保存失败");
                }
            }
            catch (ArgumentException ex)
            {
                return Fail(ApiErrorCode.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(ApiErrorCode.BusinessRuleViolation, $"保存失败: {ex.Message}");
            }
        }

    }
}
