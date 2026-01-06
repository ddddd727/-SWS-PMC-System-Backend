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
        private readonly PmcContext _context;
        private readonly IMapper _mapper;

        public PmcSpecService(PmcContext context, IMapper mapper)
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
            if (string.IsNullOrWhiteSpace(PmcCode))
            {
                throw new ArgumentException("PMC编码不能为空");
            }

            // 确保为7位编码
            var singleCodes = PmcCode.ToArray();
            if (singleCodes.Length != 7)
            {
                throw new Exception("输入的编码不为7位");
            }

            // 从数据库中查找对应PMC编码的数据
            var entity = _context.S3dRulePmcdata
                .AsNoTracking()
                .FirstOrDefault(x => x.Pmccode == PmcCode);

            if (entity == null)
            {
                throw new Exception($"未找到PMC编码 {PmcCode} 对应的数据");
            }

            // 将实体数据映射到基础信息DTO
            var baseInfo = new PmcBaseInfoDto
            {
                PmcCode = entity.Pmccode,
                ShipNumber = entity.ShipNo,
                Status = entity.Status ?? string.Empty,
                PipingClass = entity.PipingClassName,
                MaterialGrade = entity.MaterialsGradeName,
                PressureRating = entity.PressureRatingName,
                PipeStandard = entity.PipingStandardName,
                MaterialCategory = entity.MaterialsCategoryName,
                WallThickness = entity.ScheduleThicknessName
            };

            return baseInfo;
        }

        /// <summary>
        /// 根据端面标准和壁厚系列获取通径、外径、壁厚信息
        /// </summary>
        /// <param name="EndStandard">端面标准</param>
        /// <param name="Schedule">壁厚系列</param>
        /// <returns>通径、外径、壁厚信息</returns>
        public SpecNPDInfoDto GetNPDInfoByPmc(string EndStandard, string Schedule)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(EndStandard) || string.IsNullOrWhiteSpace(Schedule))
            {
                throw new ArgumentException("端面标准和壁厚系列不能为空");
            }

            // 将字符串参数转换为 int（假设参数是代码值的字符串形式）
            // 如果转换失败，可能需要通过 CodeList 表查找对应的 CodeListNumber
            if (!int.TryParse(EndStandard, out int endStandardCl) || !int.TryParse(Schedule, out int scheduleCl))
            {
                throw new ArgumentException("端面标准或壁厚系列格式不正确，无法转换为整数值");
            }

            // 查询数据库中符合条件的数据
            var queryResult = _context.S3dCommonPlainPipingGenericData
                .Where(x => x.EndStandardCl == endStandardCl && x.ScheduleCl == scheduleCl)
                .AsNoTracking()
                .ToList();

            // 构建返回结果
            var result = new SpecNPDInfoDto
            {
                EndStandard = EndStandard,
                Schedule = Schedule,
                NPD = queryResult
                    .Where(x => x.NominalPipingDiameter > 0)
                    .Select(x => (double)x.NominalPipingDiameter)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                OutsideDiameter = queryResult
                    .Where(x => x.PipingOutsideDiameter.HasValue && x.PipingOutsideDiameter.Value > 0)
                    .Select(x => (double)x.PipingOutsideDiameter!.Value)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                WallThickness = queryResult
                    .Where(x => x.WallThickness.HasValue && x.WallThickness.Value > 0)
                    .Select(x => (double)x.WallThickness!.Value)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList()
            };

            return result;
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
