using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int RoomsNumber { get; set; }
        public int EmptyRoomsNumber { get; set; }

        public virtual LocationEntity Location { get; set; }
        public virtual List<RoomEntity> Rooms { get; set; } = new List<RoomEntity>();
        public virtual List<GuestEntity> Guests { get; set; } = new List<GuestEntity>();
    }
}
