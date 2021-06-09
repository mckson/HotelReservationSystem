using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity : Entity
    {
        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public double Deposit { get; set; }

        public string Description { get; set; }

        // public IEnumerable<byte[]> Images { get; set; }
        public byte[] MainImage { get; set; }

        public virtual LocationEntity Location { get; set; }

        public virtual IEnumerable<RoomEntity> Rooms { get; set; }

        public virtual IEnumerable<HotelUserEntity> HotelUsers { get; set; }

        public virtual IEnumerable<ReservationEntity> Reservations { get; set; }

        public virtual IEnumerable<ServiceEntity> Services { get; set; }
    }
}
