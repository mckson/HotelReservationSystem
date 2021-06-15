using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Filters
{
    public class HotelsFilter
    {
        public Guid? ManagerId { get; set; }

        public DateTime? DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public IEnumerable<string> Services { get; set; }
    }
}
