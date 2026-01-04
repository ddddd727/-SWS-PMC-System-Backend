using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Models;
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



    }
}
