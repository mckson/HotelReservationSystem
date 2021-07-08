using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.Account
{
    public class AuthenticateUserCommand : IRequest<TokenResponseModel>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
