using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dDictPipingBendDataService
    {
        Task<List<S3dDictPipingBendDataDto>> GetAllListAsync();
        Task<S3dDictPipingBendDataDto> CreateAsync(CreateS3dDictPipingBendDataDto dto);
        Task<bool> UpdateAsync(UpdateS3dDictPipingBendDataDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
