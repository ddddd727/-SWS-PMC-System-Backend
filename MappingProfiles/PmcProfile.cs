using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class PmcProfile : Profile
    {
        public PmcProfile()
        {
            CreateMap<DspSpmcDictPipingBend, DspSpmcDictPipingBendDto>().ReverseMap();
            CreateMap<CreateDspSpmcDictPipingBendDto, DspSpmcDictPipingBend>();
            CreateMap<UpdateDspSpmcDictPipingBendDto, DspSpmcDictPipingBend>();
        }
    }
}
