using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        //public RoomType RoomType { get; set; }
        public int SeatNumber { get; set; } //quantity of мест (rename later) 
    }
}
