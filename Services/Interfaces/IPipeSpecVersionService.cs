using PMCSystem_Backend.Dtos.PipeSpecConfig;
using PMCSystem_Backend.Entities.PipeSpecConfig;

namespace PMCSystem_Backend.Services.Interfaces;

/// <summary>
/// 管系规格书版本管理服务接口
/// </summary>
public interface IPipeSpecVersionService
{
    /// <summary>
    /// 获取历史版本列表（分页）
    /// </summary>
    /// <param name="pmcCode">PMC 编码</param>
    /// <param name="shipType">船型（可选，与 shipNumber 同时提供时精确匹配）</param>
    /// <param name="shipNumber">船号（可选）</param>
    /// <param name="pageIndex">页码，从 1 开始</param>
    /// <param name="pageSize">每页条数</param>
    /// <returns>版本列表及总数</returns>
    (List<PipeSpecVersionDto> Items, int TotalCount) GetVersionList(string pmcCode, string? shipType, string? shipNumber, int pageIndex = 1, int pageSize = 20);

    /// <summary>
    /// 获取历史版本详情
    /// </summary>
    /// <param name="versionId">版本记录主键 Id</param>
    /// <returns>版本详情，未找到则返回 null</returns>
    PipeSpecVersionDetailDto? GetVersionDetail(int versionId);

    /// <summary>
    /// 使用历史版本覆盖当前版本
    /// </summary>
    /// <param name="versionId">版本记录主键 Id</param>
    /// <param name="shipType">船型（可选，与 shipNumber 同时提供时精确匹配主表记录）</param>
    /// <param name="shipNumber">船号（可选）</param>
    /// <returns>是否成功</returns>
    bool RevertToVersion(int versionId, string? shipType = null, string? shipNumber = null);

    /// <summary>
    /// 创建版本快照（在保存主表前调用，将当前主表数据复制到历史表）
    /// </summary>
    /// <param name="currentEntity">主表当前实体</param>
    /// <param name="createdBy">创建人（可选）</param>
    /// <param name="comment">版本备注（可选）</param>
    void CreateVersionSnapshot(S3dRulePmcData currentEntity, string? createdBy = null, string? comment = null);
}
