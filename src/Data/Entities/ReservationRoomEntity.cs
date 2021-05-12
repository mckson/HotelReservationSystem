namespace HotelReservation.Data.Entities
{
    public class ReservationRoomEntity
    {
        public int ReservationId { get; set; }

        public virtual ReservationEntity Reservation { get; set; }

        public int RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }
    }
}
