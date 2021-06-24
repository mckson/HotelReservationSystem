using HotelReservation.Business.Models;
using HotelReservation.Data.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IRoomsService : IBaseService<RoomModel>, IUpdateService<RoomModel>
    {
        IEnumerable<RoomModel> GetAllRooms();

        Task<int> GetCountAsync(RoomsFilter filter);

        IEnumerable<RoomModel> GetPagedRooms(
            PaginationFilter paginationFilter,
            RoomsFilter filter);
    }
}
