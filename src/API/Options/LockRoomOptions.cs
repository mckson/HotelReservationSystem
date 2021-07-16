namespace HotelReservation.API.Options
{
    public class LockRoomOptions
    {
        public const string LockRoomSetting = "RoomLockSettings";

        public int LockTimeInMinutes { get; set; }

        public int UnauthorizedLockTimeInMinutes { get; set; }
    }
}
