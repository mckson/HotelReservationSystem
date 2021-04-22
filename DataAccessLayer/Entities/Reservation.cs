using System;

namespace DataAccessLayer.Entities
{
    class Reservation
    {
        //Primary Key
        public int Id { get; set; }
        //Foreign Key
        public int GuestId { get; set; }
        //Foreign Key
        public int RoomId { get; set; }
        //public List<Room> Rooms { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public int TotalDays { get; set; }
        //public int TotalRooms { get; set; }
        public decimal Deposit { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
