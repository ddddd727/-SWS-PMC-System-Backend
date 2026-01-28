using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IDspSpmcDictPipingBendDataService
    {
        Task<List<DspSpmcDictPipingBendDataDto>> GetAllListAsync();
        Task<DspSpmcDictPipingBendDataDto> CreateAsync(CreateDspSpmcDictPipingBendDataDto dto);
        Task<bool> UpdateAsync(UpdateDspSpmcDictPipingBendDataDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
