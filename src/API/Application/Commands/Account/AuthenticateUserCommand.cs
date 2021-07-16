using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Account
{
    public class AuthenticateUserCommand : IRequest<TokenResponseModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
