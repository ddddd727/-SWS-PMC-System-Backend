using PMCSystem_Backend.Modules.PipingSpecifications.Entities;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IPmcCodeService
    {
        IEnumerable<PmcCodeGenerateResponseItem> GenerateWithDescriptions(IEnumerable<string> pmcCodes);
    IEnumerable<PmcCodeQueryItem> GetPmcCodes(string shipType, string shipNo);
    IEnumerable<PmcOptionDto> GetOptions(string type, string? parentDesc = null);
    void SavePmcCodes(PmcCodeSaveRequest request);
    int DeletePmcCodes(PmcCodeDeleteRequest request);
    List<ShipInfo> GetShipInfos();
    int CopyRules(CopyRuleRequest request);
    }
}
