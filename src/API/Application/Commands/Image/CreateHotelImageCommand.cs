using MediatR;

namespace HotelReservation.API.Application.Commands.Image
{
    public class CreateHotelImageCommand : IRequest
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsMain { get; set; }

        public string HotelId { get; set; }
    }
}
