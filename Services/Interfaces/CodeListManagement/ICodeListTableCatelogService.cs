using PMCSystem_Backend.Dtos.CodeListManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces.CodeListManagement
{
    public interface ICodeListTableCatelogService
    {
        Task<List<CodeListTableCatelogDto>> GetAllAsync();
        Task<CodeListTableCatelogDto> CreateAsync(CreateCodeListTableCatelogDto dto);
        Task<CodeListHierarchyDto> GetHierarchyNamesAsync(string codeListTableName);
        Task<MultiLevelCodeListResponseDto> GetMultiLevelCodeListValuesAsync(MultiLevelCodeListRequestDto request);
        Task<CodeListCombinedResponseDto> GetCombinedCodeListAsync(string codeListTableName);
        Task<List<CodeListValueDto>> GetCodeListValuesByParentShortStringValueAsync(string shortStringValue);
    }
}
