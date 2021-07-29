using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class RoomFacilityRepository : BaseRepository<RoomFacilityEntity>, IRoomFacilityRepository
    {
        public RoomFacilityRepository(HotelContext context, ISortHelper<RoomFacilityEntity> sortHelper)
            : base(context, sortHelper)
        {
        }
    }
}
