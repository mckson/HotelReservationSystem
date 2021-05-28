using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>
    {
        public RoomRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
