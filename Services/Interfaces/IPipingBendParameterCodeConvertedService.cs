using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IPipingBendParameterCodeConvertedService
    {
        Task<List<PipingBendParameterCodeConvertedDto>> GetAllAsync();
    }
}

