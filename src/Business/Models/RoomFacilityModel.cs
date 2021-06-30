using System;

namespace HotelReservation.Business.Models
{
    public class RoomFacilityModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid RoomId { get; set; }

        public virtual RoomModel Room { get; set; }
    }
}
