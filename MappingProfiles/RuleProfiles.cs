using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class RuleProfiles : Profile
    {
        public RuleProfiles()
        {
            CreateMap<S3dRuleAb2b3c2, S3dRuleAb2b3c2Dto>();
        }
    }
}
