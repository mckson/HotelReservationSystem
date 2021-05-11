using System;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            IAccountService accountService,
            IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserResponseModel>> Authenticate([FromBody] UserAuthenticationRequestModel userAuthRequestModel)
        {
            var userAuthModel = _mapper.Map<UserAuthenticationModel>(userAuthRequestModel);
            var loggedUser = await _accountService.AuthenticateAsync(userAuthModel);

            if (loggedUser == null)
                return BadRequest(new { errorText = "Invalid email or password" });

            var responseUser = _mapper.Map<UserResponseModel>(loggedUser);
            SetTokenCookie(responseUser.RefreshToken);

            return Ok(responseUser);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<UserResponseModel>> Register(UserRegistrationRequestModel userRequestModel)
        {
            var userModel = _mapper.Map<UserRegistrationModel>(userRequestModel);
            var registeredUserAuth = await _accountService.RegisterAsync(userModel);

            if (registeredUserAuth == null)
                return BadRequest(new { errorText = "User with such email exists" });

            return await Authenticate(_mapper.Map<UserAuthenticationRequestModel>(registeredUserAuth));
        }

        [AllowAnonymous]
        [HttpPost("Refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var response = await _accountService.RefreshToken(refreshToken);

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            var responseUser = _mapper.Map<UserResponseModel>(response);
            SetTokenCookie(responseUser.RefreshToken);

            return Ok(responseUser);
        }

        [HttpPost("Revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _accountService.RevokeTokenAsync(token);

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(4)
            };
            Response.Cookies.Append("RefreshToken", token, cookieOptions);
        }
    }
}
