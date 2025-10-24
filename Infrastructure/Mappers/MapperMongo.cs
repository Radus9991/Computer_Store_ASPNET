using AutoMapper;
using ApplicationCore.Models;
using Infrastructure.Mongo.Models;


namespace Infrastructure.Mappers
{
    public class MapperMongo : Profile
    {
        public MapperMongo()
        {
            CreateMap<UserMongo, User>().ReverseMap();
            CreateMap<ComputerMongo, Computer>().ReverseMap();
            CreateMap<OrderMongo, Order>().ReverseMap();
        }
    }
}
