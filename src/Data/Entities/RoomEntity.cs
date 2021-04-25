namespace HotelReservation.Data.Entities
{
    public class RoomEntity
    {
        public int Id { get; set; }

        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsEmpty { get; set; }

        public int HotelId { get; set; }
        public HotelEntity Hotel { get; set; }

        public ReservationEntity Reservation { get; set; }
        public GuestEntity Guest { get; set; }
    }
}