using System;
using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class RoomViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<RoomRoomViewModel> RoomViews { get; set; }
    }
}
