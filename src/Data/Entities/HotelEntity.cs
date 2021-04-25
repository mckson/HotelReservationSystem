using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int RoomsNumber { get; set; }
        public int EmptyRoomsNumber { get; set; }

        public LocationEntity Location { get; set; }
        public List<RoomEntity> Rooms { get; set; } = new List<RoomEntity>();
        public List<GuestEntity> Guests { get; set; } = new List<GuestEntity>();
    }
}
