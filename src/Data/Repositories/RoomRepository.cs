using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
