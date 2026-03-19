using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.DesignRule;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Dtos.DesignRule;

namespace PMCSystem_Backend.MappingProfiles
{
    public class PmcProfile : Profile
    {
        public PmcProfile()
        {
            CreateMap<S3dDictPipingBendData, S3dDictPipingBendDataDto>().ReverseMap();
            CreateMap<CreateS3dDictPipingBendDataDto, S3dDictPipingBendData>();
            CreateMap<UpdateS3dDictPipingBendDataDto, S3dDictPipingBendData>();
            CreateMap<S3dCodePipingBendParameter, S3dCodePipingBendParameterDto>();
            CreateMap<S3dCodePlainPipingGenericData, S3dCodePlainPipingGenericDataDto>();
            CreateMap<S3dCodeShortCodeMap, S3dCodeShortCodeMapDto>();
            CreateMap<PMCSystem_Backend.Entities.PipeSpecConfig.S3dDictPipingComponentType, S3dDictPipingComponentTypeDto>().ReverseMap();

            CreateMap<S3dRuleShortCodeMap, S3dRuleShortCodeMapDto>().ReverseMap();
            CreateMap<CreateS3dRuleShortCodeMapDto, S3dRuleShortCodeMap>();
            CreateMap<UpdateS3dRuleShortCodeMapDto, S3dRuleShortCodeMap>();

            CreateMap<S3dRuleShortCodeHierarchyRule, S3dRuleShortCodeHierarchyRuleDto>().ReverseMap();
            CreateMap<CreateS3dRuleShortCodeHierarchyRuleDto, S3dRuleShortCodeHierarchyRule>();
            CreateMap<UpdateS3dRuleShortCodeHierarchyRuleDto, S3dRuleShortCodeHierarchyRule>();
            CreateMap<S3dRulePipingBendParameter, S3dRulePipingBendParameterDto>().ReverseMap();
            CreateMap<CreateS3dRulePipingBendParameterDto, S3dRulePipingBendParameter>();
            CreateMap<UpdateS3dRulePipingBendParameterDto, S3dRulePipingBendParameter>();
            CreateMap<S3dRulePipingBendParameter, UpdateS3dRulePipingBendParameterDto>();

            CreateMap<S3dCommonPlainPipingGenericData, S3dCommonPlainPipingGenericDataDto>().ReverseMap();
            CreateMap<CreateS3dCommonPlainPipingGenericDataDto, S3dCommonPlainPipingGenericData>();
            CreateMap<UpdateS3dCommonPlainPipingGenericDataDto, S3dCommonPlainPipingGenericData>();

            // CodeListTableCatelog Mappings
            CreateMap<PMCSystem_Backend.Entities.PipeSpecConfig.S3dCommonCodeListTable, CodeListTableCatelogDto>().ReverseMap();
            CreateMap<CreateCodeListTableCatelogDto, PMCSystem_Backend.Entities.PipeSpecConfig.S3dCommonCodeListTable>();
        }
    }
}
