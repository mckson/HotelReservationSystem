using System;

namespace HotelReservation.Data.Entities
{
    public class ReservationRoomEntity
    {
        public Guid ReservationId { get; set; }

        public virtual ReservationEntity Reservation { get; set; }

        public Guid RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }
    }
}
