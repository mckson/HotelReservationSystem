using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class ServiceResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public Guid HotelId { get; set; }
    }
}
