using System;

namespace HotelReservation.Data.Filters
{
    public class RoomsFilter
    {
        public int HotelId { get; set; }

        public DateTime? DateIn { get; set; }

        public DateTime? DateOut { get; set; }
    }
}
