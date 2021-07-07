using AutoMapper;
using HotelReservation.API.Commands.Hotel;
using HotelReservation.API.Commands.Room;
using HotelReservation.API.Commands.Service;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Entities;
using System;
using System.Linq;

namespace HotelReservation.API
{
    public class MappingApiModelsProfile : Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.ReadabilityRules",
            "SA1123:Do not place regions within elements",
            Justification = "Need to segregate method groups to increase readability")]
        public MappingApiModelsProfile(IUriService uriService)
        {
            #region HotelMaps
            CreateMap<HotelRequestModel, HotelModel>();
            CreateMap<CreateHotelCommand, HotelEntity>();
            CreateMap<UpdateHotelCommand, HotelEntity>();
            CreateMap<HotelEntity, HotelResponseModel>()
                .ForMember(
                    response => response.Managers,
                    opt => opt.MapFrom(model =>
                        model.HotelUsers.Select(hu => hu.User)))
                .ForMember(
                    response => response.MainImage,
                    options => options.MapFrom(model =>
                        uriService.GetResourceUri(Endpoints.HotelImages, model.Images.FirstOrDefault(image => image.IsMain).Id.ToString())))
                .ForMember(
                    response => response.Images,
                    options => options.MapFrom(model =>
                        model.Images.Select(image =>
                            uriService.GetResourceUri(Endpoints.HotelImages, image.Id.ToString()))));
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
            #endregion

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

            #region ServiceMaps
            CreateMap<ServiceRequestModel, ServiceModel>();
            CreateMap<ServiceRequestModel, ServiceEntity>();
            CreateMap<CreateServiceCommand, ServiceEntity>();
            CreateMap<ServiceModel, ServiceResponseModel>();
            CreateMap<ServiceEntity, ServiceResponseModel>();
            CreateMap<ServiceModel, ServiceBriefResponseModel>();
            CreateMap<ServiceEntity, ServiceBriefResponseModel>();
            #endregion

            #region RoomMaps
            CreateMap<CreateRoomCommand, RoomEntity>()
                .ForMember(
                    entity => entity.RoomViews,
                    options => options.MapFrom(request =>
                        request.Views.Select(roomViewId => new RoomRoomViewEntity() { RoomViewId = roomViewId })))
                .ForMember(
                    model => model.Facilities,
                    options => options.MapFrom(request =>
                        request.Facilities.Select(facility => new RoomFacilityEntity() { Name = facility })));
            CreateMap<UpdateRoomCommand, RoomEntity>()
                .ForMember(
                    entity => entity.RoomViews,
                    options => options.MapFrom(request =>
                        request.Views.Select(roomViewId => new RoomRoomViewEntity() { RoomViewId = roomViewId })))
                .ForMember(
                    model => model.Facilities,
                    options => options.MapFrom(request =>
                        request.Facilities.Select(facility => new RoomFacilityEntity() { Name = facility })));
            CreateMap<RoomEntity, RoomResponseModel>()
                .ForMember(
                    response => response.Reservations,
                    opt => opt.MapFrom(entity =>
                        entity.ReservationRooms.Select(rr => rr.ReservationId)))
                .ForMember(
                    response => response.Images,
                    options => options.MapFrom(entity =>
                        entity.Images.Select(image =>
                            uriService.GetResourceUri(Endpoints.RoomImages, image.Id.ToString()))))
                .ForMember(
                    response => response.Views,
                    options => options.MapFrom(entity =>
                        entity.RoomViews.Select(rv => rv.RoomView)));
            CreateMap<RoomModel, RoomBriefResponseModel>();
            #endregion

            #region  LocationMaps
            CreateMap<LocationModel, LocationResponseModel>();
            CreateMap<LocationEntity, LocationResponseModel>();
            CreateMap<LocationRequestModel, LocationModel>();
            CreateMap<LocationRequestModel, LocationEntity>();
            #endregion

            CreateMap<RoomFacilityEntity, RoomFacilityResponseModel>();

            #region RoomViewMaps
            CreateMap<RoomViewRequestModel, RoomViewEntity>();
            CreateMap<RoomViewRequestModel, RoomViewModel>();
            CreateMap<RoomViewEntity, RoomViewResponseModel>();
            CreateMap<RoomViewModel, RoomViewResponseModel>();
            #endregion

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

            #region UserMaps
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
            CreateMap<UserEntity, UserBriefResponseModel>()
                .ForMember(
                    response => response.Hotels,
                    opt => opt.MapFrom(model => model.HotelUsers.Select(hu => hu.HotelId)));
            CreateMap<UserModel, UserResponseModel>()
                .ForMember(
                    response => response.Hotels,
                    opt => opt.MapFrom(model => model.HotelUsers.Select(hu => hu.HotelId)));
            #endregion
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
