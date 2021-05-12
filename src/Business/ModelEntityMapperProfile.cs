using AutoMapper;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business
{
    public class ModelEntityMapperProfile : Profile
    {
        public ModelEntityMapperProfile()
        {
            CreateMap<HotelEntity, HotelModel>().ReverseMap();
            CreateMap<LocationEntity, LocationModel>().ReverseMap();
            CreateMap<RoomEntity, RoomModel>().ReverseMap();
            CreateMap<RefreshTokenEntity, RefreshTokenModel>().ReverseMap();
            CreateMap<HotelServiceEntity, HotelServiceModel>().ReverseMap();
            CreateMap<ServiceEntity, ServiceModel>().ReverseMap();
        }
    }
}