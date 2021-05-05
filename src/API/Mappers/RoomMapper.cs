using AutoMapper;
using HotelReservation.Data.Entities;

namespace HotelReservation.API.Mappers
{
    public class RoomMapper/*: IMapper<RoomEntity, RoomResponseModel, RoomRequestModel>*/
    {
        /*private readonly Mapper _mapper;

        public RoomMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomEntity, RoomResponseModel>()
                    .ForMember(
                        "HotelUrl",
                        opt => opt.MapFrom(entity => $"https://localhost:5001/api/Hotels/{entity.HotelId}"))
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
        }*/
    }
}
