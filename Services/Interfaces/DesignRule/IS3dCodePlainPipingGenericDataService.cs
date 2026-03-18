using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodePlainPipingGenericDataService
    {
        Task<List<S3dCodePlainPipingGenericDataDto>> GetAllAsync();
    }
}
