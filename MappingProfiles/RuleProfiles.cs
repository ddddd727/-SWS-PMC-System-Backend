using AutoMapper;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class RuleProfiles : Profile
    {
        public RuleProfiles()
        {
            CreateMap<DspSpmcRuleAb2b3c2, DspSpmcRuleAb2b3c2Dto>();
        }
    }
}
