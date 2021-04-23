using System;

namespace HotelReservation.Data.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }

        public int GuestId { get; set; }
        public GuestEntity Guest { get; set; }

        public int RoomId { get; set; }
        public RoomEntity Room { get; set; }
        
        public DateTime ReservationDate { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public int TotalDays { get; set; }
        public double Deposit { get; set; }
        public double TotalPrice { get; set; }
    }
}
