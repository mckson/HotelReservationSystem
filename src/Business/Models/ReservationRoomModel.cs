using System;

namespace HotelReservation.Business.Models
{
    public class ReservationRoomModel
    {
        public Guid ReservationId { get; set; }

        public ReservationModel Reservation { get; set; }

        public Guid RoomId { get; set; }

        public RoomModel Room { get; set; }
    }
}
