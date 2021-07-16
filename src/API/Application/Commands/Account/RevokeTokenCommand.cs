using MediatR;

namespace HotelReservation.API.Application.Commands.Account
{
    public class RevokeTokenCommand : IRequest
    {
        public string Token { get; set; }
    }
}
