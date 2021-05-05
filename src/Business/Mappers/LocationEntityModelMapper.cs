using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Mappers
{
    public class LocationEntityModelMapper : IEntityModelMapper<LocationEntity, LocationModel>
    {
        public LocationModel EntityToModel(LocationEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public LocationEntity ModelToEntity(LocationModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
