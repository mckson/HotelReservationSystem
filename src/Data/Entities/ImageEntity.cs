namespace HotelReservation.Data.Entities
{
    public abstract class ImageEntity : Entity
    {
        public byte[] Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
