namespace HotelReservation.Business
{
    public class AuthenticationOptions
    {
        public const string Authentication = "Authentication";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public double Lifetime { get; set; }
    }
}