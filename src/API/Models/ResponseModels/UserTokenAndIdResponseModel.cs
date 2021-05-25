using System;

namespace HotelReservation.API.Models.ResponseModels
{
    public class UserTokenAndIdResponseModel
    {
        public UserResponseModel User { get; set; }

        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
