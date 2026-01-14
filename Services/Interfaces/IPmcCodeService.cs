using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IPmcCodeService
    {
        IEnumerable<PmcCodeGenerateResponseItem> GenerateWithDescriptions(IEnumerable<string> pmcCodes);
    }
}

