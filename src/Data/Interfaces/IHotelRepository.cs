using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IHotelRepository : IRepository<HotelEntity>
    {
        Task<HotelEntity> GetAsync(string name);
    }
}
