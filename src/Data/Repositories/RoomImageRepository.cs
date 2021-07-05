using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class RoomImageRepository : BaseRepository<RoomImageEntity>, IRoomImageRepository
    {
        public RoomImageRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
