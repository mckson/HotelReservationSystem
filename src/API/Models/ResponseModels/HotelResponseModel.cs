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

        public IEnumerable<UserResponseModel> Managers { get; set; }

        public IEnumerable<ServiceResponseModel> Services { get; set; }

        public IEnumerable<ReservationResponseModel> Reservations { get; set; }
    }
}