namespace HotelReservation.API.Options
{
    public class UnregisteredUserOptions
    {
        public const string UnregisteredUserSettings = "UnregisteredUserSettings";

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
