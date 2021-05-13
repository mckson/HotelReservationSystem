using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class ServiceRequestModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int HotelId { get; set; }
    }
}
