using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int RoomsNumber { get; set; }
        //public List<Room> Rooms { get; set; }
    }
}
