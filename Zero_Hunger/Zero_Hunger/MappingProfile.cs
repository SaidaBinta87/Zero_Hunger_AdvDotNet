using AutoMapper;
using Zero_Hunger.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zero_Hunger.EF;

namespace Zero_Hunger
{   public static class Mapping
    {

        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();

            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;

}
    public class MappingProfile:Profile
        {
        public MappingProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<CollectRequest, CollectRequestDTO>().ReverseMap();
        }
    }
    
}

