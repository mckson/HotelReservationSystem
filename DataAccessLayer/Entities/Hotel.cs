using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    class Hotel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int RoomsNumber { get; set; }

        //One To One
        public Location Location { get; set; }

        //One To Many
        public List<Room> Rooms { get; set; } = new List<Room>();

        //OneToMany
        public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}
