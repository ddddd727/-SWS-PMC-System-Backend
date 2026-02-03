using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class PmcProfile : Profile
    {
        public PmcProfile()
        {
            CreateMap<S3dDictPipingBendData, S3dDictPipingBendDataDto>().ReverseMap();
            CreateMap<CreateS3dDictPipingBendDataDto, S3dDictPipingBendData>();
            CreateMap<UpdateS3dDictPipingBendDataDto, S3dDictPipingBendData>();
            CreateMap<S3dCodeWallThickness, WallThicknessCodeConvertedDto>();
            CreateMap<S3dCodePipingBendParameter, PipingBendParameterCodeConvertedDto>();

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
