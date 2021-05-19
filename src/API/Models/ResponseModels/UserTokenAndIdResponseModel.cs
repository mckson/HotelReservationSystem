using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class UserTokenAndIdResponseModel
    {
        public Guid Id { get; set; }

        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
