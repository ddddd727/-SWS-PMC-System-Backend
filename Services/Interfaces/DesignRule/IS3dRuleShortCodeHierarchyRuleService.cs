using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dRuleShortCodeHierarchyRuleService
    {
        Task<List<S3dRuleShortCodeHierarchyRuleDto>> GetAllListAsync();
        Task<S3dRuleShortCodeHierarchyRuleDto> CreateAsync(CreateS3dRuleShortCodeHierarchyRuleDto dto);
        Task<bool> UpdateAsync(UpdateS3dRuleShortCodeHierarchyRuleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
