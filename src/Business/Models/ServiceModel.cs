using System;
using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class ServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public Guid HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public IEnumerable<ReservationServiceModel> ReservationServices { get; set; }
    }
}
