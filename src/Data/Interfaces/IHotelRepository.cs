using HotelReservation.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
    public interface IHotelRepository : IRepository<HotelEntity>
    {
        Task<HotelEntity> GetAsync(string name);

        IEnumerable<string> GetAllHotelsUniqueNames();
    }
}
