using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ServiceRepository : BaseRepository<ServiceEntity>, IServiceRepository
    {
        public ServiceRepository(HotelContext context, ISortHelper<ServiceEntity> sortHelper)
            : base(context, sortHelper)
        {
        }
    }
}
