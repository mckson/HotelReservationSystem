using System.Collections.Generic;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelsService : IBaseService<HotelEntity, HotelModel>
    {
        public IEnumerable<HotelModel> GetHotels();
    }
}
