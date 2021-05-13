using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using System.Linq;

namespace HotelReservation.API
{
    public class MappingApiModelsProfile : Profile
    {
        public MappingApiModelsProfile()
        {
            CreateMap<HotelRequestModel, HotelModel>();
            CreateMap<HotelModel, HotelResponseModel>();

            CreateMap<ServiceRequestModel, ServiceModel>();
            CreateMap<ServiceModel, ServiceResponseModel>()
                .ForMember(
                response => response.HotelName,
                opt => opt.MapFrom(model => model.Hotel.Name));

            CreateMap<RoomRequestModel, RoomModel>();
            CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(
                    response => response.HotelName,
                    opt => opt.MapFrom(model => model.Hotel.Name));

            CreateMap<LocationModel, LocationResponseModel>();
            CreateMap<LocationRequestModel, LocationModel>();

            CreateMap<ReservationModel, ReservationResponseModel>()
                .ForMember(
                    response => response.HotelName,
                    opt => opt.MapFrom(model => model.Hotel.Name))
                .ForMember(
                    response => response.UserName,
                    opt => opt.MapFrom(model => model.User.UserName))
                .ForMember(
                    response => response.Rooms,
                    opt => opt.MapFrom(model => model.ReservationRooms.Select(rr => rr.Room)))
                .ForMember(
                    response => response.Services,
                    opt => opt.MapFrom(model => model.ReservationServices.Select(rs => rs.Service)));
            CreateMap<ReservationRequestModel, ReservationModel>();

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
