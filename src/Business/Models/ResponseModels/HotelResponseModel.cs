using System.Collections.Generic;

namespace HotelReservation.Business.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public string HotelName { get; set; }

        public string CompanyName { get; set; }

        public LocationResponseModel Location { get; set; }

        public IEnumerable<RoomResponseModel> Rooms { get; set; }
    }
}
