using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class ImagesRepository : BaseRepository<ImageEntity>
    {
        public ImagesRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
