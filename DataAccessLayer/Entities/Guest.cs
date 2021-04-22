using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    class Guest : Person
    {
        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }
        //Related entity
        public Room Room { get; set; }

        [ForeignKey(nameof(Hotel))]
        public int HotelId { get; set; }
        //Related Entity
        public Hotel Hotel { get; set; }

        public Reservation Reservation { get; set; }
    }
}
