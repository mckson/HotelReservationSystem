using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Room
    {
        public int Id { get; set; }
        
        public int HotelId { get; set; }
        //Connected entity - when deleted Hotel there all related users will be deleted too
        public Hotel Hotel { get; set; }

        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        //public RoomType RoomType { get; set; }
        public int Capacity { get; set; } //quantity of мест (rename later) 
        public bool IsEmpty { get; set; }

        public Reservation Reservation { get; set; }

        //One To Many
        //public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}
