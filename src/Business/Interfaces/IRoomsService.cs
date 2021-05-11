using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IRoomsService : IBaseService<RoomEntity, RoomModel>
    {
        IEnumerable<RoomModel> GetAllRooms();

        Task<IEnumerable<RoomModel>> GetRoomsByHotelNameAsync(string hotelName);

        Task<IEnumerable<RoomModel>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
