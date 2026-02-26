using PMCSystem_Backend.Dtos.Dict;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IDictService
    {
        // 🔍 修改：增加 keyword 参数，默认可为空
        Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null);

        Task<int> AddAsync(string type, DictInputDto data);
        Task<int> UpdateAsync(string type, int id, DictInputDto data);
        Task<int> DeleteAsync(string type, int id);
    }
}