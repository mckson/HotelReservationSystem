using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;

namespace HotelReservation.API
{
    public class MappingApiModelsProfile : Profile
    {
        public MappingApiModelsProfile()
        {
            CreateMap<HotelRequestModel, HotelModel>();
            CreateMap<HotelModel, HotelResponseModel>();

            CreateMap<RoomRequestModel, RoomModel>();
            CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(
                    response => response.HotelName,
                    opt => opt.MapFrom(model => model.Hotel.Name));

            CreateMap<LocationModel, LocationResponseModel>();
            CreateMap<LocationRequestModel, LocationModel>();

            CreateMap<UserRegistrationRequestModel, UserRegistrationModel>()
                .ReverseMap();
            CreateMap<UserAuthenticationRequestModel, UserAuthenticationModel>()
                .ReverseMap();
            CreateMap<UserAdminCreationRequestModel, UserRegistrationModel>();
            CreateMap<UserUpdateRequestModel, UserUpdateModel>();
            CreateMap<UserModel, UserResponseModel>()
                .ForMember(
                    response => response.RefreshToken,
                    opt => opt.MapFrom(model => model.RefreshToken.Token));
        }
    }
}
