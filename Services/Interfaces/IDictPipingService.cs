using System.Collections.Generic;
using System.Threading.Tasks;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IDictPipingService
    {
        Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null);
        Task<int> AddAsync(string type, DictInputDto data);
        Task<int> UpdateAsync(string type, int id, DictInputDto data);
        Task<int> DeleteAsync(string type, int id);
        Task<int> BatchDeleteAsync(string type, List<int> ids);
        Task<List<dynamic>> GetComponentTypeListAsync();
        Task<IEnumerable<dynamic>> GetGeoStandardOptionsAsync(string type);
    }
}