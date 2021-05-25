using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        [HttpPost("SignIn")]
        public async Task<ActionResult<UserTokenAndIdResponseModel>> Authenticate([FromBody] UserAuthenticationRequestModel userAuthRequestModel)
        {
            var userAuthModel = _mapper.Map<UserAuthenticationModel>(userAuthRequestModel);
            var loggedUser = await _accountService.AuthenticateAsync(userAuthModel);

            var responseUser = _mapper.Map<UserTokenAndIdResponseModel>(loggedUser);
            SetTokenCookie(responseUser.RefreshToken);

            return Ok(responseUser);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ActionResult<UserTokenAndIdResponseModel>> Register(UserRegistrationRequestModel userRequestModel)
        {
            var userModel = _mapper.Map<UserRegistrationModel>(userRequestModel);
            var registeredUserAuth = await _accountService.RegisterAsync(userModel);

            return await Authenticate(_mapper.Map<UserAuthenticationRequestModel>(registeredUserAuth));
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<UserTokenAndIdResponseModel>> RefreshToken([FromBody] RefreshTokenRequestModel refreshToken)
        {
            // var refreshToken = Request.Cookies["RefreshToken"];
            var response = await _accountService.RefreshToken(refreshToken.Token);

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            var responseUser = _mapper.Map<UserTokenAndIdResponseModel>(response);
            SetTokenCookie(responseUser.RefreshToken);

            return Ok(responseUser);
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> RevokeTokenAsync()
        {
            // accept token from request body or cookie
            var token = Request.Cookies["refreshToken"];

            await _accountService.RevokeTokenAsync(token);

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
