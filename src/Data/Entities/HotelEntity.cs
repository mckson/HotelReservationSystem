using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual LocationEntity Location { get; set; }

        public virtual IEnumerable<RoomEntity> Rooms { get; set; }

        public virtual IEnumerable<UserEntity> Users { get; set; }
    }
}
