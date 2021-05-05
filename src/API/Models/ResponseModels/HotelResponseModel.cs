namespace HotelReservation.API.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public string HotelName { get; set; }

        // public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }

        // public LocationResponseModel Location { get; set; }
        public string LocationUrl { get; set; }

        // public IEnumerable<RoomResponseModel> Rooms { get; set; }
        public string RoomsUrl { get; set; }
    }
}
