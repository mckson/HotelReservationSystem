using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    class Reservation
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [ForeignKey(nameof(Guest))]
        [Required]
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        
        [ForeignKey(nameof(Room))]
        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        //public List<Room> Rooms { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public DateTime DateIn { get; set; }

        [Required]
        public DateTime DateOut { get; set; }

        [Required]
        public int TotalDays { get; set; }
        //public int TotalRooms { get; set; }

        [Required]
        public decimal Deposit { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
