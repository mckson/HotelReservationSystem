using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(HotelContext context)
            : base(context)
        {
        }

        public IEnumerable<string> GetAllRoomsUniqueNames()
        {
            var names = DbSet.Where(room => room.Name != null && room.Name != string.Empty).Select(room => room.Name).Distinct();
            return names;
        }

        public IEnumerable<string> GetHotelRoomsUniqueNames(Guid hotelId)
        {
            var names = DbSet.Where(room => room.Name != null && room.Name != string.Empty && room.HotelId.Value.Equals(hotelId)).Select(room => room.Name).Distinct();
            return names;
        }

        public IEnumerable<int> GetHotelRoomsUniqueNumbers(Guid hotelId)
        {
            var names = DbSet.Where(room => room.HotelId.Value.Equals(hotelId)).Select(room => room.RoomNumber).Distinct();
            return names;
        }
    }
}
