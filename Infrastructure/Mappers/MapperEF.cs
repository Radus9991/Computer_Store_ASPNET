using AutoMapper;
using Infrastructure.EF.Models;
using ApplicationCore.Models;


namespace Infrastructure.Mappers
{
    public class MapperEF : Profile
    {
        public MapperEF()
        {
            CreateMap<UserEntity, User>().ReverseMap();
            CreateMap<ComputerEntity, Computer>().ReverseMap();
            CreateMap<OrderEntity, Order>().ReverseMap();
        }
    }
}
