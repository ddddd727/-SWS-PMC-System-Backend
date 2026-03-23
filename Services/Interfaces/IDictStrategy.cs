using PMCSystem_Backend.Modules.StandardComponents.Dtos;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IDictStrategy
    {
        Task<int> AddAsync(string type, DictItemConfig config, DictInputDto data);
        Task<int> UpdateAsync(string type, int id, DictItemConfig config, DictInputDto data);
        Task<int> DeleteAsync(string type, int id, DictItemConfig config);
    }
}