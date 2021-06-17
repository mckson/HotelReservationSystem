namespace HotelReservation.API.Models.ResponseModels
{
    public class ImageResponseModel
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int HotelId { get; set; }
    }
}
