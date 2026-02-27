using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dDictWallThicknessService
    {
        Task<List<S3dDictWallThicknessDto>> GetAllListAsync();
        Task<S3dDictWallThicknessDto> CreateAsync(CreateS3dDictWallThicknessDto dto);
        Task<bool> UpdateAsync(UpdateS3dDictWallThicknessDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
