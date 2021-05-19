namespace HotelReservation.Data.Entities
{
    public class ReservationServiceEntity
    {
        public int ReservationId { get; set; }

        public virtual ReservationEntity Reservation { get; set; }

        public int ServiceId { get; set; }

        public virtual ServiceEntity Service { get; set; }
    }
}
