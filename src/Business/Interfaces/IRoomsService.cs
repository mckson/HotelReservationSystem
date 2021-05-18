using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using System.Collections.Generic;

namespace HotelReservation.Business.Interfaces
{
    public interface IRoomsService : IBaseService<RoomEntity, RoomModel>
    {
        IEnumerable<RoomModel> GetAllRooms();
    }
}
