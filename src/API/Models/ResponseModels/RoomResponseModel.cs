using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomResponseModel
    {
        public Guid Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public double Price { get; set; }

        public Guid HotelId { get; set; }

        public IEnumerable<Guid> Reservations { get; set; }

        public IEnumerable<Uri> Images { get; set; }
    }
}
