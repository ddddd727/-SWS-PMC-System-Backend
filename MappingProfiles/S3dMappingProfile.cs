using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class S3dMappingProfile : Profile
    {
        public S3dMappingProfile() 
        {
            // S3dRuleAb2b3c2 Mappings
            CreateMap<S3dRuleAb2b3c2, S3dRuleAb2b3c2Dto>().ReverseMap();

            // S3dRuleC1c2 Mappings
            CreateMap<S3dRuleC1c2, S3dRuleC1c2Dto>().ReverseMap();

            // S3dRuleB1b2b3d Mappings
            CreateMap<S3dRuleB1b2b3d, S3dRuleB1b2b3dDto>().ReverseMap();
        } 
    }
}
