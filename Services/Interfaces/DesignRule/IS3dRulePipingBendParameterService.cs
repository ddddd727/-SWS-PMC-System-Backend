using PMCSystem_Backend.Modules.DesignRules.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dRulePipingBendParameterService
    {
        Task<List<S3dRulePipingBendParameterDto>> GetAllListAsync();
        Task<S3dRulePipingBendParameterDto> CreateAsync(CreateS3dRulePipingBendParameterDto dto);
        Task<bool> UpdateAsync(UpdateS3dRulePipingBendParameterDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
