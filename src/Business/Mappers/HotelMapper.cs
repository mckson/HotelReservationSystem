using System.Linq;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Mappers
{
    public class HotelMapper : IMapper<HotelEntity, HotelResponseModel, HotelRequestModel>
    {
        private readonly Mapper _mapper;

        public HotelMapper(LocationMapper locationMapper, RoomMapper roomMapper)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HotelEntity, HotelResponseModel>()
                    .ForMember("HotelName", opt => opt.MapFrom(entity => entity.Name))
                    .ForMember(
                        "Location",
                        opt => opt.MapFrom(entity => locationMapper.EntityToResponse(entity.Location)))
                    .ForMember(
                        "Rooms",
                        opt => opt.MapFrom(entity => entity.Rooms.Select(r => roomMapper.EntityToResponse(r))))
                    .ForMember("CompanyName", opt => opt.MapFrom(entity => entity.Company.Title));

                cfg.CreateMap<HotelRequestModel, HotelEntity>()
                    .ForMember("Name", opt => opt.MapFrom(request => request.HotelName));
            });

            _mapper = new Mapper(configuration);
        }

        public HotelResponseModel EntityToResponse(HotelEntity entityModel)
        {
            return _mapper.Map<HotelEntity, HotelResponseModel>(entityModel);
        }

        public HotelResponseModel RequestToResponse(HotelRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }

        public HotelEntity RequestToEntity(HotelRequestModel requestModel)
        {
            return _mapper.Map<HotelRequestModel, HotelEntity>(requestModel);
        }
    }
}
