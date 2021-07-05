using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using System;
using System.Linq;

namespace HotelReservation.API
{
    public class MappingApiModelsProfile : Profile
    {
        public MappingApiModelsProfile(IUriService uriService)
        {
            CreateMap<HotelRequestModel, HotelModel>();
            CreateMap<HotelModel, HotelResponseModel>()
                .ForMember(
                    response => response.Managers,
                    opt => opt.MapFrom(model =>
                        model.HotelUsers.Select(hu => hu.User)))
                .ForMember(
                    response => response.MainImage,
                    options => options.MapFrom(model =>
                        uriService.GetResourceUri(Endpoints.HotelImages, model.MainImage.Id.ToString())))
                .ForMember(
                    response => response.Images,
                    options => options.MapFrom(model =>
                        model.Images.Select(image =>
                            uriService.GetResourceUri(Endpoints.HotelImages, image.Id.ToString()))));
            CreateMap<HotelModel, HotelBriefResponse>();

            CreateMap<HotelImageRequestModel, HotelImageModel>()
                .ForMember(
                    model => model.Image,
                    options => options.MapFrom(request => ConvertBase64ToBytes(request.Image)));
            CreateMap<HotelImageModel, HotelImageResponseModel>()
                .ForMember(
                    response => response.Image,
                    options => options.MapFrom(model => ConvertBytesToBase64(model.Image, model.Type)));

            CreateMap<RoomImageRequestModel, RoomImageModel>()
                .ForMember(
                    model => model.Image,
                    options => options.MapFrom(request => ConvertBase64ToBytes(request.Image)));
            CreateMap<RoomImageModel, RoomImageResponseModel>()
                .ForMember(
                    response => response.Image,
                    options => options.MapFrom(model => ConvertBytesToBase64(model.Image, model.Type)));

            CreateMap<ServiceRequestModel, ServiceModel>();
            CreateMap<ServiceModel, ServiceResponseModel>();
            CreateMap<ServiceModel, ServiceBriefResponseModel>();

            CreateMap<RoomRequestModel, RoomModel>()
                .ForMember(
                    model => model.RoomViews,
                    options => options.MapFrom(request =>
                        request.Views.Select(roomView => new RoomRoomViewModel { RoomViewId = Guid.Parse(roomView) })))
                .ForMember(
                    model => model.Facilities,
                    options => options.MapFrom(request =>
                        request.Facilities.Select(facility => new RoomFacilityModel { Name = facility })));
            CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(
                    response => response.Reservations,
                    opt => opt.MapFrom(model =>
                        model.ReservationRooms.Select(rr => rr.ReservationId)))
                .ForMember(
                    response => response.Images,
                    options => options.MapFrom(model =>
                        model.Images.Select(image =>
                            uriService.GetResourceUri(Endpoints.RoomImages, image.Id.ToString()))))
                .ForMember(
                    response => response.Views,
                    options => options.MapFrom(model =>
                        model.RoomViews.Select(rv => rv.RoomView)));
            CreateMap<RoomModel, RoomBriefResponseModel>();

            CreateMap<LocationModel, LocationResponseModel>();
            CreateMap<LocationRequestModel, LocationModel>();

            CreateMap<RoomFacilityModel, RoomFacilityResponseModel>();

            CreateMap<RoomViewRequestModel, RoomViewModel>();
            CreateMap<RoomViewModel, RoomViewResponseModel>();

            CreateMap<ReservationModel, ReservationResponseModel>()
                .ForMember(
                    response => response.Rooms,
                    opt => opt.MapFrom(
                        model => model.ReservationRooms.Select(rr => rr.Room)))
                .ForMember(
                    response => response.Services,
                    opt => opt.MapFrom(
                        model => model.ReservationServices.Select(rs => rs.Service)))
                .ForMember(
                    response => response.RoomsPrice,
                    options => options.MapFrom(
                        model => model.ReservationRooms.Sum(rr => rr.Room.Price)))
                .ForMember(
                    response => response.ServicesPrice,
                    options => options.MapFrom(
                        model => model.ReservationServices.Sum(rs => rs.Service.Price)));
            CreateMap<ReservationModel, ReservationBriefResponseModel>()
                .ForMember(
                    response => response.HotelName,
                    options => options.MapFrom(model => model.Hotel.Name));
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

        private static byte[] ConvertBase64ToBytes(string image)
        {
            var converted = image.Split(',')[1];
            var imageData = Convert.FromBase64String(converted);

            return imageData;
        }

        private static string ConvertBytesToBase64(byte[] image, string type)
        {
            type ??= "image/jpeg";

            var base64 = Convert.ToBase64String(image);
            var base64Full = $"data:{type};base64,{base64}";

            return base64Full;
        }
    }
}
