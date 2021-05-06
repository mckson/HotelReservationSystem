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
        public CompanyRequestModel Company { get; set; }

        [Required]
        public LocationRequestModel Location { get; set; }
    }
}
