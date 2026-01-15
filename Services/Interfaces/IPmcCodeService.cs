using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IPmcCodeService
    {
        IEnumerable<PmcCodeGenerateResponseItem> GenerateWithDescriptions(IEnumerable<string> pmcCodes);
    IEnumerable<PmcCodeQueryItem> GetPmcCodes(string shipType, string shipNo);
    void SavePmcCodes(PmcCodeSaveRequest request);
    }
}
