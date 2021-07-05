using System;

namespace HotelReservation.Business.Models
{
    public class LocationModel
    {
        public Guid Id { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int BuildingNumber { get; set; }

        public Guid HotelId { get; set; }

        public HotelModel Hotel { get; set; }
    }
}
