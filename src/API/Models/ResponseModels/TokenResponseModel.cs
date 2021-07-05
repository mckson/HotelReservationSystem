namespace HotelReservation.API.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
