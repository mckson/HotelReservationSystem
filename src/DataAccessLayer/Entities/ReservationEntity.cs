using System;

// ReSharper disable once CheckNamespace
namespace HotelReservation.Data.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public GuestEntity Guest { get; set; }
        public int RoomId { get; set; }
        public RoomEntity Room { get; set; }

        //public List<Room> Rooms { get; set; }
        
        public DateTime ReservationDate { get; set; }
        
        public DateTime DateIn { get; set; }
        
        public DateTime DateOut { get; set; }
        
        public int TotalDays { get; set; }
        //public int TotalRooms { get; set; }
        
        public decimal Deposit { get; set; }
        
        public decimal TotalPrice { get; set; }
    }
}
