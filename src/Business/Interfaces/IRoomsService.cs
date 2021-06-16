using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IRoomsService : IBaseService<RoomEntity, RoomModel>
    {
        IEnumerable<RoomModel> GetAllRooms();

        Task<int> GetCountAsync(RoomsFilter filter);

        IEnumerable<RoomModel> GetPagedRooms(
            PaginationFilter paginationFilter,
            RoomsFilter filter);
    }
}
