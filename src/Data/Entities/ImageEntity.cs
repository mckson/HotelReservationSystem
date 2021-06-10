namespace HotelReservation.Data.Entities
{
    public class ImageEntity : Entity
    {
        public byte[] Image { get; set; }

        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }
    }
}
