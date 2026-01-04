using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class PmcSpecRuleProfile : Profile
    {
        public PmcSpecRuleProfile() 
        {
            // Entity -> Dto(查询)
            CreateMap<PmcSpecRuleData, PmcSelectInfoDto>();
            
        }
    }
}
