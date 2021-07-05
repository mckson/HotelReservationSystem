namespace HotelReservation.API.Helpers
{
    public class PasswordOptions
    {
        public const string PasswordSettings = "PasswordSettings";

        public bool RequireDigit { get; set; }

        public int RequiredLength { get; set; }

        public bool RequireNonAlphanumeric { get; set; }

        public bool RequireUppercase { get; set; }

        public bool RequireLowercase { get; set; }
    }
}
