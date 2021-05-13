using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class HotelModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public LocationModel Location { get; set; }

        public IEnumerable<RoomModel> Rooms { get; set; }

        public IEnumerable<UserModel> Managers { get; set; }

        public IEnumerable<ReservationModel> Reservations { get; set; }

        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
