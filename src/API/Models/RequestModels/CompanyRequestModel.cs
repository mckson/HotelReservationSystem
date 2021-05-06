using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class CompanyRequestModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
