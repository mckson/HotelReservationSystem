using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class RoomRequestModel
    {
        [Required]
        [Range(1, 10000)]
        public int RoomNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int FloorNumber { get; set; }

        [Required]
        [Range(1, 10)]
        public int Capacity { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int HotelId { get; set; }
    }
}
