using System;

namespace HotelReservation.Data.Entities
{
    public class RoomImageEntity : ImageEntity
    {
        public Guid RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }
    }
}
