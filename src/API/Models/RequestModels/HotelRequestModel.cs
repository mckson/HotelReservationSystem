using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class HotelRequestModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1, 500)]
        public int NumberFloors { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Deposit { get; set; }

        [Required]
        public LocationRequestModel Location { get; set; }
    }
}
