using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.Business.Models.UserModels;

namespace HotelReservation.API.MappingProfiles
{
    public class UserApiMappingProfile : Profile
    {
        public UserApiMappingProfile()
        {
            CreateMap<UserRegistrationRequestModel, UserRegistrationModel>()
                .ReverseMap();
            CreateMap<UserAuthenticationRequestModel, UserAuthenticationModel>()
                .ReverseMap();
        }
    }
}
