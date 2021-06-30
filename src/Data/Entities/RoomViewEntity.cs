using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class RoomViewEntity : Entity
    {
        public string Name { get; set; }

        public virtual IEnumerable<RoomRoomViewEntity> RoomViews { get; set; }
    }
}
