using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class HotelImageRepository : BaseRepository<HotelImageEntity>, IHotelImageRepository
    {
        public HotelImageRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
