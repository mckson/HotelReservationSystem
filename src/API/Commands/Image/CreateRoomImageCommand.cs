using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.Image
{
    public class CreateRoomImageCommand : IRequest
    {
        [Required]
        public string Image { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string RoomId { get; set; }
    }
}
