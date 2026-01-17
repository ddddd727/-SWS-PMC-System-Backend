using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class PmcProfile : Profile
    {
        public PmcProfile()
        {
            CreateMap<S3dDictPipingBend, DspSpmcDictPipingBendDto>().ReverseMap();
            CreateMap<CreateDspSpmcDictPipingBendDto, S3dDictPipingBend>();
            CreateMap<UpdateDspSpmcDictPipingBendDto, S3dDictPipingBend>();
            CreateMap<S3dDictPipingBendDatum, DspSpmcDictPipingBendDataDto>().ReverseMap();
            CreateMap<CreateDspSpmcDictPipingBendDataDto, S3dDictPipingBendDatum>();
            CreateMap<UpdateDspSpmcDictPipingBendDataDto, S3dDictPipingBendDatum>();
            CreateMap<S3dDictWallThicknessCodeConverted, WallThicknessCodeConvertedDto>();
            CreateMap<S3dRulePipingBendParameterCodeConverted, PipingBendParameterCodeConvertedDto>();

            CreateMap<S3dDictWallThickness, S3dDictWallThicknessDto>().ReverseMap();
            CreateMap<CreateS3dDictWallThicknessDto, S3dDictWallThickness>();
            CreateMap<UpdateS3dDictWallThicknessDto, S3dDictWallThickness>();

            CreateMap<S3dRuleShortCodeHierarchyRule, S3dRuleShortCodeHierarchyRuleDto>().ReverseMap();
            CreateMap<CreateS3dRuleShortCodeHierarchyRuleDto, S3dRuleShortCodeHierarchyRule>();
            CreateMap<UpdateS3dRuleShortCodeHierarchyRuleDto, S3dRuleShortCodeHierarchyRule>();
            CreateMap<S3dRulePipingBendParameter, S3dRulePipingBendParameterDto>().ReverseMap();
            CreateMap<CreateS3dRulePipingBendParameterDto, S3dRulePipingBendParameter>();
            CreateMap<UpdateS3dRulePipingBendParameterDto, S3dRulePipingBendParameter>();
        }
    }
}
