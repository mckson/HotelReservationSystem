using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class HotelModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CompanyModel Company { get; set; }

        public LocationModel Location { get; set; }

        public IEnumerable<RoomModel> Rooms { get; set; }

        public IEnumerable<UserModel> Users { get; set; }
    }
}
