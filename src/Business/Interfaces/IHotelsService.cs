using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelsService : IBaseService<HotelEntity, HotelModel>
    {
        Task<int> GetCountAsync(HotelsFilter hotelsFilter);

        IEnumerable<HotelModel> GetAllHotels();

        IEnumerable<HotelModel> GetPagedHotels(PaginationFilter paginationFilter, HotelsFilter hotelsFilter);

        public Task<HotelModel> GetHotelByNameAsync(string name);
    }
}
