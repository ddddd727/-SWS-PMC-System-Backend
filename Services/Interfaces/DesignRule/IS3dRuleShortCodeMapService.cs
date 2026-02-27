using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IS3dRuleShortCodeMapService
    {
        Task<IEnumerable<S3dRuleShortCodeMapDto>> GetAllAsync();
        Task<S3dRuleShortCodeMapDto> CreateAsync(CreateS3dRuleShortCodeMapDto dto);
        Task<bool> UpdateAsync(UpdateS3dRuleShortCodeMapDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
