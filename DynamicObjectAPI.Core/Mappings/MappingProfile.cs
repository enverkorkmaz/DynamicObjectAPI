using AutoMapper;
using DynamicObjectAPI.Core.Models;
using DynamicObjectAPI.Core.DTOs;
using DynamicObjectAPI.Core.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DynamicObjectAPI.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DynamicObject, DynamicObjectDto>()
                .ReverseMap(); 
        }
    }
}
