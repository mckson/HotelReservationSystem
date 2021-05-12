using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public IEnumerable<HotelServiceModel> HotelServices { get; set; }

        public IEnumerable<ReservationServiceModel> ReservationServices { get; set; }
    }
}
