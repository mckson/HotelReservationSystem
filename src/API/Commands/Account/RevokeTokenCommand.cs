using MediatR;

namespace HotelReservation.API.Commands.Account
{
    public class RevokeTokenCommand : IRequest
    {
        public string Token { get; set; }
    }
}
