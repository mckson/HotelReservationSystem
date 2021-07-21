using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Filters
{
    public class RoomsFilter
    {
        public Guid HotelId { get; set; }

        public DateTime? DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public Guid? UserId { get; set; }

        public string Name { get; set; }

        public int? Number { get; set; }

        public int? MinFloorNumber { get; set; }

        public int? MaxFloorNumber { get; set; }

        public int? MinCapacity { get; set; }

        public int? MaxCapacity { get; set; }

        public double? MinArea { get; set; }

        public double? MaxArea { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public bool Smoking { get; set; } = false;

        public bool Parking { get; set; } = false;

        public IEnumerable<string> Facilities { get; set; }

        public IEnumerable<string> RoomViews { get; set; }
    }
}
