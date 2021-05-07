using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class UserAuthenticationRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
