using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public double Deposit { get; set; }

        public string Description { get; set; }

        public IEnumerable<ImageResponseModel> Images { get; set; }

        public ImageResponseModel MainImage { get; set; }

        public LocationResponseModel Location { get; set; }

        // public IEnumerable<RoomResponseModel> Rooms { get; set; }
        public IEnumerable<UserBriefResponseModel> Managers { get; set; }

        public IEnumerable<ServiceResponseModel> Services { get; set; }
    }
}