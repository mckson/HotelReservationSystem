using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelsService
    {
        public IEnumerable<HotelModel> GetHotels();

        /*public Task<IEnumerable<RoomResponseModel>> GetHotelRooms(int id);

        public Task<LocationResponseModel> GetHotelLocation(int id);

        public Task<CompanyResponseModel> GetHotelCompany(int id);*/
    }
}
