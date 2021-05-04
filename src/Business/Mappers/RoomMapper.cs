using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Mappers
{
    public class RoomMapper : IMapper<RoomEntity, RoomResponseModel, RoomRequestModel>
    {
        private readonly Mapper _mapper;

        public RoomMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomEntity, RoomResponseModel>()
                    .ForMember("HotelName", opt => opt.MapFrom(entity => entity.Hotel.Name))
                    .ReverseMap();
                cfg.CreateMap<RoomRequestModel, RoomResponseModel>();   // implement
            });

            _mapper = new Mapper(configuration);
        }

        public RoomResponseModel EntityToResponse(RoomEntity entityModel)
        {
            return _mapper.Map<RoomEntity, RoomResponseModel>(entityModel);
        }

        public RoomResponseModel RequestToResponse(RoomRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }

        public RoomEntity RequestToEntity(RoomRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
