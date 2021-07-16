using System;
using System.Security.AccessControl;

namespace HotelReservation.API.Models.ResponseModels
{
    public class HotelBriefResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public LocationResponseModel Location { get; set; }
    }
}
