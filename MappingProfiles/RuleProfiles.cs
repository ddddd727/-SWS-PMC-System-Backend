using AutoMapper;
using PMCSystem_Backend.Modules.PMCRuleConfig.Dtos;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.MappingProfiles
{
    public class RuleProfiles : Profile
    {
        public RuleProfiles()
        {
            CreateMap<S3dCodeAb2b3c2, PipeLimitRuleDto>();
        }
    }
}
