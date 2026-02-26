using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Dtos.PipeSpecConfig;

namespace PMCSystem_Backend.Services.Interfaces
{
    /// <summary>
    /// S3dRulePmcData 服务接口
    /// </summary>
    public interface IS3dRulePmcDataService
    {
        /// <summary>
        /// 根据ID获取单条记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>S3dRulePmcData DTO</returns>
        Task<S3dRulePmcDataDto?> GetByIdAsync(int id);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request">分页请求参数</param>
        /// <returns>分页结果</returns>
        Task<PagedResult<S3dRulePmcDataDto>> GetPagedAsync(PagedRequest request);

        /// <summary>
        /// 根据PMC编码查询
        /// </summary>
        /// <param name="pmcCode">PMC编码</param>
        /// <returns>S3dRulePmcData DTO列表</returns>
        Task<List<S3dRulePmcDataDto>> GetByPmcCodeAsync(string pmcCode);

        /// <summary>
        /// 根据船号查询
        /// </summary>
        /// <param name="shipNo">船号</param>
        /// <returns>S3dRulePmcData DTO列表</returns>
        Task<List<S3dRulePmcDataDto>> GetByShipNoAsync(string shipNo);

        /// <summary>
        /// 创建新记录
        /// </summary>
        /// <param name="dto">创建DTO</param>
        /// <returns>创建后的记录DTO</returns>
        Task<S3dRulePmcDataDto> CreateAsync(CreateS3dRulePmcDataDto dto);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="dto">更新DTO</param>
        /// <returns>是否更新成功</returns>
        Task<bool> UpdateAsync(int id, UpdateS3dRulePmcDataDto dto);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">记录ID列表</param>
        /// <returns>删除成功的数量</returns>
        Task<int> DeleteBatchAsync(List<int> ids);
    }
}
