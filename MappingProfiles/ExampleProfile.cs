using AutoMapper;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.MappingProfiles
{
    public class ExampleProfile : Profile
    {
        public ExampleProfile() 
        {
            // Entity -> Dto (查询时常用）
            CreateMap<ExampleEntity, ExampleDto>();

            // Dto -> Entity （新增/更新时常用）
            CreateMap<ExampleDto, ExampleEntity>();


        } 
    }
}
