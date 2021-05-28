using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : BaseRepository<HotelEntity>, IHotelRepository
    {
        public HotelRepository(HotelContext context)
        : base(context)
        {
        }

        public async Task<HotelEntity> GetAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(hotel => hotel.Name == name);
        }
    }
}