using System.Collections.Generic;

namespace HotelReservation.Data.Filters
{
    public class HotelsFilter
    {
        public string Name { get; set; }

        public string City { get; set; }

        public IEnumerable<string> Services { get; set; }
    }
}
