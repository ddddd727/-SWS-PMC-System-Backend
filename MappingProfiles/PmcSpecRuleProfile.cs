using AutoMapper;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;

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

            // S3dRulePmcData CRUD 映射
            // Entity -> DTO
            CreateMap<S3dRulePmcData, S3dRulePmcDataDto>();

            // CreateDTO -> Entity
            CreateMap<CreateS3dRulePmcDataDto, S3dRulePmcData>();

            // UpdateDTO -> Entity (忽略ID，因为ID在路由中)
            CreateMap<UpdateS3dRulePmcDataDto, S3dRulePmcData>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
