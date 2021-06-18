using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ImagesRepository : BaseRepository<ImageEntity>, IImageRepository
    {
        public ImagesRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
