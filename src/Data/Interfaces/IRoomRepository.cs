using HotelReservation.Data.Entities;
using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Interfaces
{
    public interface IRoomRepository : IRepository<RoomEntity>
    {
        IEnumerable<string> GetAllRoomsGetAllUniqueNames();

        IEnumerable<string> GetHotelRoomsUniqueNames(Guid hotelId);

        IEnumerable<int> GetHotelRoomsUniqueNumbers(Guid hotelId);
    }
}
