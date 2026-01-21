using PMCSystem_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IVwMaterialsCategoryScheduleThicknessService
    {
        Task<IEnumerable<ScheduleThicknessDto>> GetScheduleThicknessesByMaterialsCategoryClAsync(int materialsCategoryCl);
    }
}

