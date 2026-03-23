using PMCSystem_Backend.Modules.DesignRules.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodePlainPipingGenericDataService
    {
        Task<List<S3dCodePlainPipingGenericDataDto>> GetAllAsync();
    }
}
