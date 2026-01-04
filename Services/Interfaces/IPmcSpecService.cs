using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.Models;
using System.Net;

namespace PMCSystem_Backend.Services.Interfaces
{
    /// <summary>
    /// PMC编码配置服务
    /// </summary>
    public interface IPmcSpecService
    {
        /// <summary>
        /// 获取所有船型船号信息
        /// </summary>
        /// <returns>船型信息</returns>
        List<ShipInfo> GetShipInfos();


        /// <summary>
        /// 根据船号选择，获取PMC编码信息
        /// </summary>
        /// <param name="shipNumber">船号</param>
        /// <returns>PMC编码列表信息</returns>
        List<DspSpmcRulePmcdata> GetPmcRulesByShipNum(string shipNumber);


        /// <summary>
        /// 解析PMC编码内容
        /// </summary>
        /// <param name="PmcCode">pmc编码</param>
        /// <returns></returns>
        PMCCodeDto AnalyzeCodeFromPMC(string PmcCode);

        /// <summary>
        /// 获取PMC编码对应的基础管附件标准信息
        /// </summary>
        /// <param name="PmcCode"></param>
        /// <returns></returns>
        List<PipeFittingSpecDto> GetPipeFittingSpec(string PmcCode);


        /// <summary>
        /// 保存页面配置的管系规格书信息
        /// </summary>
        /// <param name="PmcRules">配置的附件列表</param>
        /// <returns></returns>
        bool SetSpecRules(List<DspSpmcRulePmcdata> PmcRules);
    }
}
