using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Dtos.PipeSpecConfig.Requests;
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
        /// 获取所有部件类型信息
        /// </summary>
        /// <returns>部件类型列表</returns>
        List<ComponentTypeInfoDto> GetComponentTypes();


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
        /// 解析PMC编码并返回基础信息和配置信息
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <returns>包含基础信息和配置信息的DTO</returns>
        PmcInfoWithConfigDto AnalyzeCodeFromPMCWithConfig(string pmcCode);

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
        /// 保存页面配置的管系规格书信息（完整版，包含通径范围，保留给后续模块使用）
        /// </summary>
        /// <param name="request">管系规格书保存请求</param>
        /// <returns></returns>
        bool SaveSpecRules(SavePipeSpecRequest request);

        /// <summary>
        /// 保存页面配置的管系规格书信息（简化版，仅包含标准名称和材料信息）
        /// </summary>
        /// <param name="request">简化的管系规格书保存请求</param>
        /// <returns></returns>
        bool SaveSpecRulesSimple(SavePipeSpecSimpleRequest request);


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

        /// <summary>
        /// 设置规格书配置状态
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <param name="status">状态：pending/review/approved</param>
        /// <param name="shipType">船型（可选，与 shipNumber 同时提供时精确匹配）</param>
        /// <param name="shipNumber">船号（可选）</param>
        /// <returns>是否更新成功</returns>
        bool SetSpecConfigStatus(string pmcCode, string status, string? shipType = null, string? shipNumber = null);

        /// <summary>
        /// 接受审核（占位，默认审核成功，后续接入审核系统）
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <param name="shipType">船型（可选）</param>
        /// <param name="shipNumber">船号（可选）</param>
        /// <returns>是否更新成功</returns>
        bool AcceptSpecReview(string pmcCode, string? shipType = null, string? shipNumber = null);
    }
}
