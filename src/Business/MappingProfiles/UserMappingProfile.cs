using AutoMapper;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationModel, UserEntity>();
            CreateMap<UserUpdateModel, UserEntity>();
            // CreateMap<UserAuthenticationModel, UserModel>();
            CreateMap<UserModel, UserEntity>()
                .ReverseMap();
            CreateMap<UserRegistrationModel, UserAuthenticationModel>();
            CreateMap<UserModel, UserAuthenticationModel>();
        }
    }
}
