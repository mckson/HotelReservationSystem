namespace HotelReservation.API.Models.ResponseModels
{
    public class ImageResponseModel
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }

        public int HotelId { get; set; }
    }
}
