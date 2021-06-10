using AutoMapper;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business
{
    public class ModelEntityMapperProfile : Profile
    {
        public ModelEntityMapperProfile()
        {
            CreateMap<HotelEntity, HotelModel>().ReverseMap();
            CreateMap<LocationEntity, LocationModel>().ReverseMap();
            CreateMap<RefreshTokenEntity, RefreshTokenModel>().ReverseMap();
            CreateMap<ReservationEntity, ReservationModel>().ReverseMap();
            CreateMap<ReservationRoomEntity, ReservationRoomModel>().ReverseMap();
            CreateMap<ReservationServiceEntity, ReservationServiceModel>().ReverseMap();
            CreateMap<HotelUserEntity, HotelUserModel>().ReverseMap();
            CreateMap<RoomEntity, RoomModel>().ReverseMap();
            CreateMap<ServiceEntity, ServiceModel>().ReverseMap();
            CreateMap<UserEntity, UserModel>().ReverseMap();
            CreateMap<ImageEntity, ImageModel>().ReverseMap();
            CreateMap<MainImageEntity, ImageModel>().ReverseMap();

            CreateMap<UserRegistrationModel, UserEntity>();
            CreateMap<UserUpdateModel, UserEntity>();
            // CreateMap<UserAuthenticationModel, UserModel>();
            CreateMap<UserRegistrationModel, UserAuthenticationModel>();
            CreateMap<UserModel, UserAuthenticationModel>();
        }
    }
}