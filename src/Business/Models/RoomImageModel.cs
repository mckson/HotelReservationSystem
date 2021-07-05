using System;

namespace HotelReservation.Business.Models
{
    public class RoomImageModel
    {
        public Guid Id { get; set; }

        public byte[] Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public Guid RoomId { get; set; }

        public virtual RoomModel Room { get; set; }
    }
}
