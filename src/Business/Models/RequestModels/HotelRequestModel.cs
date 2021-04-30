using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business.Models.RequestModels
{
    public class HotelRequestModel
    {
        [Required]
        [MaxLength(100)]
        public string HotelName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string Region { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Required]
        [Range(1, 1000)]
        public int BuildingNumber { get; set; }
    }
}
