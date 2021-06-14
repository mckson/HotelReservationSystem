using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class MainImagesRepository : BaseRepository<MainImageEntity>
    {
        public MainImagesRepository(HotelContext context)
            : base(context)
        {
        }
    }
}
