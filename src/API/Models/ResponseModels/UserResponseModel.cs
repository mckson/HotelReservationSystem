using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
