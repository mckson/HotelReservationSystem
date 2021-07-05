using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class LocationResponseModel
    {
        public Guid Id { get; set; }

        public Guid HotelId { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int BuildingNumber { get; set; }
    }
}
