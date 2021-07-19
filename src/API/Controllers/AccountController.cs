using HotelReservation.API.Application.Commands.Account;
using HotelReservation.API.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Method that is responsible for returning JWT and Refresh tokens for valid user credentials
        /// </summary>
        /// <param name="command">User's credentials (email and password)</param>
        /// <returns>TokenResponseModel that contains JWT and Refresh tokens</returns>
        /// <response code="200">Returns TokenResponseModel that contains JWT and Refresh tokens</response>
        /// <response code="404">When user with such email does not exist</response>
        /// <response code="415">Returns ErrorResponseModel when email or password are null or empty</response>
        /// <response code="422">When password is incorrect, or you try to sign in with unregistered user</response>
        [AllowAnonymous]
        [HttpPost("SignIn")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TokenResponseModel>> Authenticate([FromBody] AuthenticateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Method that is responsible for registration of new user, and invoking Authenticate method
        /// </summary>
        /// <param name="command">Information about user</param>
        /// <returns>TokenResponseModel that contains JWT and Refresh tokens</returns>
        /// <response code="200">Returns TokenResponseModel that contains JWT and Refresh tokens for created user</response>
        /// <response code="404">When user with such email does not exist</response>
        /// <response code="409">When user with such email or username already exists</response>
        /// <response code="415">Returns ErrorResponseModel when email or password are null or empty</response>
        /// <response code="422">When password is incorrect, or you try to sign in with unregistered user</response>
        [AllowAnonymous]
        [HttpPost("SignUp")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status422UnprocessableEntity)]
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

        /// <summary>
        /// Method that is responsible for refreshing JWT token by Refresh token
        /// </summary>
        /// <param name="command">Refresh token</param>
        /// <returns>TokenResponseModel that contains JWT and Refresh tokens</returns>
        /// <response code="200">Returns TokenResponseModel that contains JWT and Refresh tokens for created user</response>
        /// <response code="404">When user for current Refresh token does not exist, or Refresh token for user does not exist, or Refresh token is expired</response>
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TokenResponseModel>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Method that marks refresh token as revoked
        /// </summary>
        /// <param name="command">Refresh token</param>
        /// <returns>No content</returns>
        /// <response code="204">When Refresh token is successfully revoked</response>
        /// <response code="404">When user for current Refresh token does not exist, or Refresh token is expired</response>
        /// <response code="422">When Refresh token is null or empty</response>
        [AllowAnonymous]
        [HttpPost("SignOut")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
