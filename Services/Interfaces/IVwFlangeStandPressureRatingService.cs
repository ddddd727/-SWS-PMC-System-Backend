using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IVwFlangeStandPressureRatingService
    {
        Task<IEnumerable<FlangeStandardDto>> GetUniqueFlangeStandardsAsync();
        Task<IEnumerable<PressureRatingDto>> GetPressureRatingsByFlangeClAsync(int geometricIndustryStandardCl);
    }
}
