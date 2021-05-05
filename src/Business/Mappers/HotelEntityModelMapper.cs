using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Mappers
{
    public class HotelEntityModelMapper : IEntityModelMapper<HotelEntity, HotelModel>
    {
        private readonly Mapper _mapper;

        public HotelEntityModelMapper()
        {
            var configuration = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<HotelEntity, HotelModel>()
                        .ForMember(hm => hm.Location, opt => opt.Ignore());
                    cfg.CreateMap<HotelModel, HotelEntity>();
                });

            _mapper = new Mapper(configuration);
        }

        public HotelModel EntityToModel(HotelEntity entity)
        {
            return _mapper.Map<HotelModel>(entity);
        }

        public HotelEntity ModelToEntity(HotelModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
