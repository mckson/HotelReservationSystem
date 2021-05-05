using System.Collections.Generic;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface ILocationsService : IBaseService<LocationEntity, LocationRequestModel, LocationResponseModel>
    {
        IEnumerable<LocationResponseModel> GetLocations();
    }
}
