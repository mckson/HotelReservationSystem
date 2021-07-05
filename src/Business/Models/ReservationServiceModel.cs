using System;

namespace HotelReservation.Business.Models
{
    public class ReservationServiceModel
    {
        public Guid ReservationId { get; set; }

        public ReservationModel Reservation { get; set; }

        public Guid ServiceId { get; set; }

        public ServiceModel Service { get; set; }
    }
}
