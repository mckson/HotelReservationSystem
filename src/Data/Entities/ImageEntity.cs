namespace HotelReservation.Data.Entities
{
    public class ImageEntity : Entity
    {
        public byte[] Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int HotelId { get; set; }

        public bool IsMain { get; set; }

        public virtual HotelEntity Hotel { get; set; }
    }
}
