using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }
        //Related entity
        public Hotel Hotel { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }          
    }
}
