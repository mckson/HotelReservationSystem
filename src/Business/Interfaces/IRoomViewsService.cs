using HotelReservation.Business.Models;
using System.Collections.Generic;

namespace HotelReservation.Business.Interfaces
{
    public interface IRoomViewsService : IBaseService<RoomViewModel>, IUpdateService<RoomViewModel>
    {
        IEnumerable<RoomViewModel> GetAllRoomViews();
    }
}
