using MediatR;

namespace HotelReservation.API.Application.Commands.Image
{
    public class CreateRoomImageCommand : IRequest
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string RoomId { get; set; }
    }
}
