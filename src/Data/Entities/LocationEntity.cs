namespace HotelReservation.Data.Entities
{
    public class LocationEntity : Entity
    {
        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int BuildingNumber { get; set; }

        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }
    }
}
