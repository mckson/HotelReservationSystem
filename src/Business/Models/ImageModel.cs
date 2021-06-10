namespace HotelReservation.Business.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }

        public int HotelId { get; set; }

        public virtual HotelModel Hotel { get; set; }
    }
}
