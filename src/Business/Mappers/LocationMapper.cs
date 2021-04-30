using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Mappers
{
    public class LocationMapper : IMapper<LocationEntity, LocationResponseModel, LocationRequestModel>
    {
        private readonly Mapper _mapper;

        public LocationMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LocationEntity, LocationResponseModel>().ReverseMap();
                cfg.CreateMap<LocationRequestModel, LocationResponseModel>();   //implement
            });

            _mapper = new Mapper(configuration);
        }

        public LocationResponseModel EntityToResponse(LocationEntity entityModel)
        {
            return _mapper.Map<LocationEntity, LocationResponseModel>(entityModel);
        }

        public LocationResponseModel RequestToResponse(LocationRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
