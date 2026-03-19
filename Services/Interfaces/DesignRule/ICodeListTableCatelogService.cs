using PMCSystem_Backend.Dtos.DesignRule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface ICodeListTableCatelogService
    {
        Task<List<CodeListTableCatelogDto>> GetAllAsync();
        Task<CodeListTableCatelogDto> CreateAsync(CreateCodeListTableCatelogDto dto);
        Task<CodeListHierarchyDto> GetHierarchyNamesAsync(string codeListTableName);
        Task<MultiLevelCodeListResponseDto> GetMultiLevelCodeListValuesAsync(MultiLevelCodeListRequestDto request);
    }
}
