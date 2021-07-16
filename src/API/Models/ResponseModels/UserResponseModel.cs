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

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<ReservationBriefResponseModel> Reservations { get; set; }

        public IEnumerable<Guid> Hotels { get; set; }
    }
}
