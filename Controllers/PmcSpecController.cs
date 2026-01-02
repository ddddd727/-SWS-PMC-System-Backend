using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
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

    }
}
