using System.Collections.Generic;

namespace HotelReservation.Data.Filters
{
    public class UsersFilter
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
