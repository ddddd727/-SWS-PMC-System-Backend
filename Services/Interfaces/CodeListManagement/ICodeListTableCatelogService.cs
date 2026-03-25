using PMCSystem_Backend.Modules.StandardComponents.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces.CodeListManagement
{
    public interface ICodeListTableCatelogService
    {
        Task<List<CodeListTableCatelogDto>> GetAllAsync();
        Task<CodeListTableCatelogDto> CreateAsync(CreateCodeListTableCatelogDto dto);

        Task<CodeListCombinedResponseDto> GetCombinedCodeListAsync(string codeListTableName);
        Task<List<CodeListValueDto>> GetCodeListValuesByParentShortStringValueAsync(string shortStringValue);
        Task<CodeListValueDto> CreateCodeListValueAsync(CreateCodeListValueDto dto);

        Task<int> GetNextAvailableCodeListNumberAsync();
    }
}
