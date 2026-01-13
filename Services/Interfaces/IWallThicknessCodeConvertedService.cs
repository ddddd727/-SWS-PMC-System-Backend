using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IWallThicknessCodeConvertedService
    {
        Task<List<WallThicknessCodeConvertedDto>> GetAllAsync();
    }
}
