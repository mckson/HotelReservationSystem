using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
        
        public virtual IEnumerable<RoomEntity> Rooms { get; set; }
        
        public DateTime ReservationDate { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public int TotalDays { get; set; }
        public double Deposit { get; set; }
        public double TotalPrice { get; set; }
    }
}
