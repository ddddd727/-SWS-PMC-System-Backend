using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dCodePipingStandardPressureRatingService
    {
        Task<IEnumerable<PipingStandardDto>> GetUniquePipingStandardsAsync();
        Task<IEnumerable<PressureRatingDto>> GetPressureRatingsByPipingClAsync(int geometricIndustryStandardCl);
    }
}
