using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Models;

namespace HotelReservation.API
{
    public class ModelToModelApiProfile : Profile
    {
        public ModelToModelApiProfile()
        {
            CreateMap<HotelRequestModel, HotelModel>();
            CreateMap<HotelModel, HotelResponseModel>()
                .ForMember(
                    response => response.CompanyTitle,
                    opt => opt.MapFrom(model => model.Company.Title));

            CreateMap<CompanyModel, CompanyResponseModel>();
            CreateMap<CompanyRequestModel, CompanyModel>();

            CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(
                    response => response.HotelName,
                    opt => opt.MapFrom(model => model.Hotel.Name));

            CreateMap<LocationModel, LocationResponseModel>();
            CreateMap<LocationRequestModel, LocationModel>();
        }
    }
}
