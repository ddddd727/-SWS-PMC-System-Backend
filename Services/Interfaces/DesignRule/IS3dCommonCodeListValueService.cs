using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCommonCodeListValueService
    {

        Task<IEnumerable<dynamic>> GetOptionsAsync(string tableName);
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetMaterialsCategoryAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetScheduleThicknessAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetEndStandardAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetGeometricIndustryStandardAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetMaterialsGradeAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetShortCodeHierarchyClassAsync();
    }
}
