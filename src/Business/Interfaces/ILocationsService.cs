using System.Collections.Generic;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface ILocationsService : IBaseService<LocationEntity, LocationRequestModel, LocationResponseModel>
    {
        IEnumerable<LocationResponseModel> GetLocations();
    }
}
