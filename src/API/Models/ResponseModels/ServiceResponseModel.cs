namespace HotelReservation.API.Models.ResponseModels
{
    public class ServiceResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int HotelId { get; set; }

        public string HotelName { get; set; }
    }
}
