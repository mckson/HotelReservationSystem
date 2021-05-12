using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public virtual LocationEntity Location { get; set; }

        public virtual IEnumerable<RoomEntity> Rooms { get; set; }

        public virtual IEnumerable<UserEntity> Users { get; set; }

        public virtual IEnumerable<ReservationEntity> Reservations { get; set; }

        public virtual IEnumerable<HotelServiceEntity> HotelServices { get; set; }
    }
}
