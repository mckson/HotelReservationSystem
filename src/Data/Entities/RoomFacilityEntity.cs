using System;

namespace HotelReservation.Data.Entities
{
    public class RoomFacilityEntity : Entity
    {
        public string Name { get; set; }

        public Guid RoomId { get; set; }

        public virtual RoomEntity Room { get; set; }
    }
}
