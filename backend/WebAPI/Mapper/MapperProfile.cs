using AutoMapper;
using Infrastructure.EF.Models;
using ApplicationCore.Models;
using WebAPI.DTO.Computer;
using WebAPI.DTO.User;
using WebAPI.DTO.Order;
using ApplicationCore.DTO;

namespace WebAPI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ComputerCreateDTO, Computer>().ReverseMap();

            CreateMap<Computer, ComputerGetDTO>();

            CreateMap<UserCreateDTO, User>();
            CreateMap<Order, OrderGetDTO>();

            CreateMap<Page<Computer>, Page<ComputerGetDTO>>();
            CreateMap<Page<Order>, Page<OrderGetDTO>>();

            CreateMap<Order, OrderShortGetDTO>();
            CreateMap<Page<Order>, Page<OrderShortGetDTO>>();

        }
    }
}
