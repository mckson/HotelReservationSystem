using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public int LocationId { get; set; }

        public LocationResponseModel Location { get; set; }

        public IEnumerable<RoomResponseModel> Rooms { get; set; }

        public IEnumerable<UserResponseModel> Users { get; set; }

        public IEnumerable<ServiceInHotelResponseModel> HotelServices { get; set; }
    }
}