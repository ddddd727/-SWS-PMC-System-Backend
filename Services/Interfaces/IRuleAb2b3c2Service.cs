using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IRuleAb2b3c2Service
    {
        IEnumerable<S3dRuleAb2b3c2Dto> GetAll();
        S3dRuleAb2b3c2Dto? GetById(int id);
    }
}
