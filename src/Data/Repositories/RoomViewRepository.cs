using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class RoomViewRepository : BaseRepository<RoomViewEntity>, IRoomViewRepository
    {
        public RoomViewRepository(HotelContext context, ISortHelper<RoomViewEntity> sortHelper)
            : base(context, sortHelper)
        {
        }
    }
}
