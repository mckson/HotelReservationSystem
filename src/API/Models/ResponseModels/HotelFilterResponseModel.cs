using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class HotelFilterResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }
}
