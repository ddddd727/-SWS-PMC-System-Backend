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

            // S3dRulePmcData -> PmcSelectInfoDto (根据船号查询PMC数据)
            CreateMap<S3dRulePmcData, PmcSelectInfoDto>()
                .ForMember(dest => dest.PmcCode, opt => opt.MapFrom(src => src.Pmccode))
                .ForMember(dest => dest.ShipNumber, opt => opt.MapFrom(src => src.ShipNo))
                .ForMember(dest => dest.PipeStadard, opt => opt.MapFrom(src => src.PipeStandard))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
