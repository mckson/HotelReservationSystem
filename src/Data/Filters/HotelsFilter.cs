using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Filters
{
    public class HotelsFilter : OrderByFilter
    {
        public Guid? ManagerId { get; set; }

        public DateTime? DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public double? MinDeposit { get; set; }

        public double? MaxDeposit { get; set; }

        public double? MinFloors { get; set; }

        public double? MaxFloors { get; set; }

        public IEnumerable<string> Services { get; set; }
    }
}
