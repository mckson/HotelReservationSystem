using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class RoomViewRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}
