using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Guest /*: Person*/
    {
        public int Id { get; set; }

        //One To One, navigational property
        public User User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        //[ForeignKey(nameof(Room))]
        //public int RoomId { get; set; }
        ////Related entity
        //public Room Room { get; set; }

        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }
        //Related Entity
        public Hotel Hotel { get; set; }

        public Reservation Reservation { get; set; }
    }
}
