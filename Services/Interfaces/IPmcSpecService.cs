using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;
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
        List<PMCSystem_Backend.Entities.PipeSpecConfig.ShipInfo> GetShipInfos();


        /// <summary>
        /// 根据船号选择，获取PMC编码信息
        /// </summary>
        /// <param name="shipNumber">船号</param>
        /// <returns>PMC编码列表信息</returns>
        List<PmcSelectInfoDto> GetPmcRulesByShipNum(string shipNumber);


        /// <summary>
        /// 解析PMC编码内容
        /// </summary>
        /// <param name="PmcCode">pmc编码</param>
        /// <returns></returns>
        PmcBaseInfoDto AnalyzeCodeFromPMC(string PmcCode);

        /// <summary>
        /// 根据PMC内包含的标准信息和壁厚系列获取对应的通径范围
        /// </summary>
        /// <param name="EndStandard"> 端面标准 </param>
        /// <param name="Schedule"> 壁厚系列 </param>
        /// <returns></returns>
        SpecNPDInfoDto GetNPDInfoByPmc(string EndStandard, string Schedule);



        /// <summary>
        /// 获取PMC编码对应的基础管附件标准信息
        /// </summary>
        /// <param name="compnentType"> 部件类型 </param>
        /// <returns></returns>
        List<PipeFittingSpecDto> GetPipeFittingSpec(string compnentType);


        /// <summary>
        /// 保存页面配置的管系规格书信息
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <param name="standardInfos">配置的标准信息列表</param>
        /// <returns></returns>
        bool SaveSpecRules(string pmcCode, List<PmcStandardInfo> standardInfos);


        /// <summary>
        /// 获取保存配置的管系规格书信息
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <param name="standardInfos">输出的标准信息列表</param>
        /// <returns></returns>
        bool GetSpecRules(string pmcCode, out List<PmcStandardInfo> standardInfos);


        /// <summary>
        /// 生成对应的管系规格书
        /// </summary>
        /// <returns></returns>
        bool GeneratePipeSpecTable();
    }
}
