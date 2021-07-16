﻿namespace HotelReservation.API.Models.RequestModels
{
    public class LocationRequestModel
    {
        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int BuildingNumber { get; set; }
    }
}
