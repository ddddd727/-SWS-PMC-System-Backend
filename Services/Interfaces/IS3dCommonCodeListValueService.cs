using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCommonCodeListValueService
    {
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetMaterialsCategoryAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetScheduleThicknessAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetEndStandardAsync();
    }
}
