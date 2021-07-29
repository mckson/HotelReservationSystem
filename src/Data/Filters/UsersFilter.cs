using System.Collections.Generic;

namespace HotelReservation.Data.Filters
{
    public class UsersFilter : OrderByFilter
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
    }
}
