using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    class Location
    {
        //Primary Key
        public int Id { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }          
    }
}
