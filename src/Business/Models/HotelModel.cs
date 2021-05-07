using System.Collections.Generic;
using HotelReservation.Business.Models.UserModels;

namespace HotelReservation.Business.Models
{
    public class HotelModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LocationModel Location { get; set; }

        public IEnumerable<RoomModel> Rooms { get; set; }

        public IEnumerable<UserModel> Users { get; set; }
    }
}
