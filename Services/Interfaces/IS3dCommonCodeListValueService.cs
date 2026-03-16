using PMCSystem_Backend.Models;
// ÐÂÔö
using PMCSystem_Backend.Dtos.Dict;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCommonCodeListValueService
    {

        Task<IEnumerable<dynamic>> GetOptionsAsync(string tableName, DataSourceRelationConfig? relation = null);
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetMaterialsCategoryAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetScheduleThicknessAsync();
        Task<IEnumerable<S3dCommonCodeListValueDto>> GetEndStandardAsync();
    }
}
