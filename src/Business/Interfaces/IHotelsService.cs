using HotelReservation.Business.Models;
using HotelReservation.Data.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelsService : IBaseService<HotelModel>, IUpdateService<HotelModel>
    {
        Task<int> GetCountAsync(HotelsFilter hotelsFilter);

        Task<IEnumerable<HotelModel>> GetAllHotelsAsync();

        Task<IEnumerable<HotelModel>> GetPagedHotelsAsync(PaginationFilter paginationFilter, HotelsFilter hotelsFilter);

        public Task<HotelModel> GetHotelByNameAsync(string name);
    }
}
