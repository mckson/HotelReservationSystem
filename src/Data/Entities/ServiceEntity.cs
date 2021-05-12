using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class ServiceEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public virtual IEnumerable<HotelServiceEntity> HotelServices { get; set; }

        public virtual IEnumerable<ReservationServiceEntity> ReservationServices { get; set; }
    }
}
