using System;

namespace HotelReservation.Data.Entities
{
    public class ReservationServiceEntity
    {
        public Guid ReservationId { get; set; }

        public virtual ReservationEntity Reservation { get; set; }

        public Guid ServiceId { get; set; }

        public virtual ServiceEntity Service { get; set; }
    }
}
