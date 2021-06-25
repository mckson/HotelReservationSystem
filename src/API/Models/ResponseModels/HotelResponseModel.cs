using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public double Deposit { get; set; }

        public string Description { get; set; }

        public IEnumerable<Uri> Images { get; set; }

        public Uri MainImage { get; set; }

        public LocationResponseModel Location { get; set; }

        public IEnumerable<UserBriefResponseModel> Managers { get; set; }

        public IEnumerable<ServiceResponseModel> Services { get; set; }
    }
}