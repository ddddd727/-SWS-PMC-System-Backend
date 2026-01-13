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
        }
    }
}
