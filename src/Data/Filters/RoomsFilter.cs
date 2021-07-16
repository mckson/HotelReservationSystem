using System;

namespace HotelReservation.Data.Filters
{
    public class RoomsFilter
    {
        public Guid HotelId { get; set; }

        public DateTime? DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public Guid? UserId { get; set; }
    }
}
