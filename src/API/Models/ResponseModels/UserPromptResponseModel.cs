using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class UserPromptResponseModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
