using System;
using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class UserBriefResponseModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<int> Hotels { get; set; }
    }
}
