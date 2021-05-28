using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class ServiceEntity : Entity
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int HotelId { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        public virtual IEnumerable<ReservationServiceEntity> ReservationServices { get; set; }
    }
}
