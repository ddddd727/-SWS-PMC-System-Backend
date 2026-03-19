using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCommonPlainPipingGenericDataService
    {
        Task<List<S3dCommonPlainPipingGenericDataDto>> GetAllListAsync();
        Task<S3dCommonPlainPipingGenericDataDto> CreateAsync(CreateS3dCommonPlainPipingGenericDataDto dto);
        Task<bool> UpdateAsync(UpdateS3dCommonPlainPipingGenericDataDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
