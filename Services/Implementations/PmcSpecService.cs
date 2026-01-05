using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class PmcSpecService : IPmcSpecService
    {
        private readonly PmcNewContext _context;
        private readonly IMapper _mapper;

        public PmcSpecService(PmcNewContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 解析PMCcode内容，返回7位编码的解析结果
        /// </summary>
        /// <param name="PmcCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public PmcBaseInfoDto AnalyzeCodeFromPMC(string PmcCode)
        {
            // 拆分7位编码
            var singleCodes = PmcCode.ToArray();
            PmcBaseInfoDto baseInfo = new PmcBaseInfoDto();
            // 编码格式校验
            if (singleCodes.Length != 7)
            {
                throw new Exception("输入的编码不为7位");
            }
            // 开发阶段使用测试数据测试接口跑通，后续将更改为对PMC编码的实际解析。
            baseInfo.PmcCode = PmcCode;
            baseInfo.WallThickness = "SCH20";
            baseInfo.PipeStandard = "GB/T 8163";
            baseInfo.Status = "UnApproved";
            baseInfo.PressureRating = "PN 6";
            baseInfo.MaterialGrade = "碳钢";
            return baseInfo;
        }

        public SpecNPDInfoDto GetNPDInfoByPmc(string EndStandard, string Schedule)
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
        /// <param name="shipNumber">船号</param>
        /// <returns>PMC编码列表信息</returns>
        public List<PmcSelectInfoDto> GetPmcRulesByShipNum(string shipNumber)
        {
            // 根据船号查询数据库中的PMC数据
            var pmcDataList = _context.S3dRulePmcdata
                .Where(x => x.ShipNo == shipNumber)
                .AsNoTracking()
                .ToList();

            // 使用AutoMapper将实体类转换为DTO
            var result = _mapper.Map<List<PmcSelectInfoDto>>(pmcDataList);

            return result;
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

        public bool SetSpecRules(List<PmcSpecInfoDto> PmcRules)
        {
            throw new NotImplementedException();
        }
    }
}
