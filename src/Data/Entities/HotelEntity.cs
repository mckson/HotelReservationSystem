using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public int RoomsNumber => Rooms.Count();
        //public int EmptyRoomsNumber => Rooms.Count(room => room.IsEmpty);
        //public IEnumerable<RoomEntity> EmptyRooms => Rooms.Where(room => room.IsEmpty);

        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }

        public virtual LocationEntity Location { get; set; }
        public virtual IEnumerable<RoomEntity> Rooms { get; set; } 
        public virtual IEnumerable<GuestEntity> Guests { get; set; }
    }
}
