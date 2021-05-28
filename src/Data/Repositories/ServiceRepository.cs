using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class ServiceRepository : BaseRepository<ServiceEntity>
    {
        public ServiceRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
