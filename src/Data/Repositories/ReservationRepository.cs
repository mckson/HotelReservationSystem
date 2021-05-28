using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class ReservationRepository : BaseRepository<ReservationEntity>
    {
        public ReservationRepository(HotelContext context)
            : base(context)
        {
        }
    }
}