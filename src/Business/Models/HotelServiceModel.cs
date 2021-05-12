namespace HotelReservation.Business.Models
{
    public class HotelServiceModel
    {
        public int HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public int ServiceId { get; set; }

        public ServiceModel Service { get; set; }
    }
}
