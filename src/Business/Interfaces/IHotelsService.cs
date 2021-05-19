using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelsService : IBaseService<HotelEntity, HotelModel>
    {
        public IEnumerable<HotelModel> GetHotels();

        public Task<HotelModel> GetHotelByNameAsync(string name);
    }
}
