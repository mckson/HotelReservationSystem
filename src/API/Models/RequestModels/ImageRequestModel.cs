using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class ImageRequestModel
    {
        [Required]
        public string Image { get; set; }

        public int HotelId { get; set; }
    }
}
