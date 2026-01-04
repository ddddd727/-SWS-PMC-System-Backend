using PMCSystem_Backend.Common.Models;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IDspSpmcDictPipingBendService
    {
        Task<List<DspSpmcDictPipingBendDto>> GetAllListAsync();
        Task<DspSpmcDictPipingBendDto?> GetByIdAsync(int id);
        Task<DspSpmcDictPipingBendDto> CreateAsync(CreateDspSpmcDictPipingBendDto dto);
        Task<bool> UpdateAsync(UpdateDspSpmcDictPipingBendDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
