using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class PmcSpecService : IPmcSpecService
    {
        private readonly PmcTestContext _context;

        public PmcSpecService(PmcTestContext context)
        {
            _context = context;
        }

        public PMCCodeDto AnalyzeCodeFromPMC(string PmcCode)
        {
            throw new NotImplementedException();
        }

        public List<PipeFittingSpecDto> GetPipeFittingSpec(string PmcCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据船号获取PMC数据
        /// </summary>
        /// <param name="shipNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<DspSpmcRulePmcdata> GetPmcRulesByShipNum(string shipNumber)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有船型船号信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ShipInfo> GetShipInfos()
        {
            // 从外部接口获取船型船号，目前先暂时用模拟数据代替
            List<ShipInfo> shipInfos = new List<ShipInfo>
            {
                new ShipInfo { shipNumber = "H1508", shipType = "邮轮" },
                new ShipInfo { shipNumber = "H1509", shipType = "邮轮" },
                new ShipInfo { shipNumber = "H1403", shipType = "民船" },
                new ShipInfo { shipNumber = "H1404", shipType = "民船" },
                new ShipInfo { shipNumber = "H1301", shipType = "货船" },
                new ShipInfo {shipNumber = "H1603", shipType = "民船" }
            };
            return shipInfos;
        }

        public bool SetSpecRules(List<DspSpmcRulePmcdata> PmcRules)
        {
            throw new NotImplementedException();
        }
    }
}
