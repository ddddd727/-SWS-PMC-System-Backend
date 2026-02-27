using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodeWallThicknessService
    {
        Task<List<S3dCodeWallThicknessDto>> GetAllAsync();
    }
}
