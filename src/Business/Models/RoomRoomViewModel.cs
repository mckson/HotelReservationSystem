using System;

namespace HotelReservation.Business.Models
{
    public class RoomRoomViewModel
    {
        public Guid RoomId { get; set; }

        public virtual RoomModel Room { get; set; }

        public Guid RoomViewId { get; set; }

        public virtual RoomViewModel RoomView { get; set; }
    }
}
