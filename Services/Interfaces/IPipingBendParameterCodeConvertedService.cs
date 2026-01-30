using PMCSystem_Backend.Dtos.DesignRules;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IPipingBendParameterCodeConvertedService
    {
        Task<List<PipingBendParameterCodeConvertedDto>> GetAllAsync();
    }
}

