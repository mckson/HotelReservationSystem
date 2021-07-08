using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.Image
{
    public class CreateHotelImageCommand : IRequest
    {
        [Required]
        public string Image { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public bool IsMain { get; set; }

        [Required]
        public string HotelId { get; set; }
    }
}
