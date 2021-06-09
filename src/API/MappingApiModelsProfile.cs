using System;
using System.IO;
using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace HotelReservation.API
{
    public class MappingApiModelsProfile : Profile
    {
        public MappingApiModelsProfile()
        {
            CreateMap<HotelRequestModel, HotelModel>()
                .ForMember(
                    model => model.MainImage,
                    options => options.MapFrom(request => ImageConverter(request.MainImage)));
            CreateMap<HotelModel, HotelResponseModel>()
                .ForMember(
                    response => response.Managers,
                    opt => opt.MapFrom(model => model.HotelUsers.Select(hu => hu.User)));

            CreateMap<ServiceRequestModel, ServiceModel>();
            CreateMap<ServiceModel, ServiceResponseModel>();

            CreateMap<RoomRequestModel, RoomModel>();
            CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(
                    response => response.Reservations,
                    opt => opt.MapFrom(model => model.ReservationRooms.Select(rr => rr.ReservationId)));

            CreateMap<LocationModel, LocationResponseModel>();
            CreateMap<LocationRequestModel, LocationModel>();

            CreateMap<ReservationModel, ReservationResponseModel>()
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
            CreateMap<UserModel, UserPrivateResponseModel>()
                .ForMember(
                    response => response.RefreshToken,
                    opt => opt.MapFrom(model => model.RefreshToken.Token));

            CreateMap<UserModel, TokenResponseModel>()
                .ForMember(
                    response => response.RefreshToken,
                    opt => opt.MapFrom(model => model.RefreshToken.Token));

            CreateMap<UserModel, UserBriefResponseModel>()
                .ForMember(
                    response => response.Hotels,
                    opt => opt.MapFrom(model => model.HotelUsers.Select(hu => hu.HotelId)));
            CreateMap<UserModel, UserResponseModel>()
                .ForMember(
                response => response.Hotels,
                opt => opt.MapFrom(model => model.HotelUsers.Select(hu => hu.HotelId)));
        }

        private static byte[] ImageConverter(string image)
        {
            var imageData = Convert.FromBase64String(image);

            return imageData;
        }
    }
}
