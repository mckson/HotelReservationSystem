using System;

namespace HotelReservation.Data.Entities
{
    public class RoomRoomViewEntity
    {
        public Guid RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }

        public Guid RoomViewId { get; set; }

        public virtual RoomViewEntity RoomView { get; set; }
    }
}
