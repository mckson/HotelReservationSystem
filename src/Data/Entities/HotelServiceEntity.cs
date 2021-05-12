namespace HotelReservation.Data.Entities
{
    public class HotelServiceEntity
    {
        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public int ServiceId { get; set; }

        public virtual ServiceEntity Service { get; set; }
    }
}
