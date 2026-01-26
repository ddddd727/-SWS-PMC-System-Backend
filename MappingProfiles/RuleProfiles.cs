using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

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
