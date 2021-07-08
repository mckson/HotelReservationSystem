using HotelReservation.API.Commands.Account;
using HotelReservation.API.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ActionResult<TokenResponseModel>> Authenticate([FromBody] AuthenticateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ActionResult<TokenResponseModel>> Register([FromBody] RegisterUserCommand command)
        {
            await _mediator.Send(command);

            var authenticateCommand = new AuthenticateUserCommand
            {
                Email = command.Email,
                Password = command.Password
            };

            var result = await Authenticate(authenticateCommand);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<TokenResponseModel>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("SignOut")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenCommand command)
        {
            await _mediator.Send(command);

            return Ok(new { message = "Token revoked" });
        }
    }
}
