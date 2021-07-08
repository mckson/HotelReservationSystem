using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Commands.Account
{
    public class RefreshTokenCommand : IRequest<TokenResponseModel>
    {
        public string Token { get; set; }
    }
}
