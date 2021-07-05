using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class ServiceBriefResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }
    }
}
