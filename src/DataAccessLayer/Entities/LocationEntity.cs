// ReSharper disable once CheckNamespace
namespace HotelReservation.Data.Entities
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public HotelEntity Hotel { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }          
    }
}
