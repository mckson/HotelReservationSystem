namespace HotelReservation.Business.Models
{
    public class ReservationRoomModel
    {
        public int ReservationId { get; set; }

        public ReservationModel Reservation { get; set; }

        public int RoomId { get; set; }

        public RoomModel Room { get; set; }
    }
}
