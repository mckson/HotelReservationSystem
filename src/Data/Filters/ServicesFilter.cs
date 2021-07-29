using System;

namespace HotelReservation.Data.Filters
{
    public class ServicesFilter : OrderByFilter
    {
        public Guid HotelId { get; set; }

        public string Name { get; set; }
    }
}
