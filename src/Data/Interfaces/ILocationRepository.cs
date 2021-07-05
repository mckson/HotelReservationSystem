using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface ILocationRepository : IRepository<LocationEntity>
    {
        public Task<LocationEntity> GetAsync(
            string country,
            string region,
            string city,
            string street,
            int building);
    }
}
