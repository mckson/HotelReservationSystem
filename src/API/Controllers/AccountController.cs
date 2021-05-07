using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelReservation.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(
            IAccountService accountService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate(UserAuthenticationRequestModel userAuthRequestModel)
        {
            var userAuthModel = _mapper.Map<UserAuthenticationModel>(userAuthRequestModel);
            var loggedUser = await _accountService.AuthenticateAsync(userAuthModel);

            if (loggedUser == null)
                return BadRequest(new { errorText = "Invalid email or password" });

            var claims = await _accountService.GetIdentityAsync(userAuthModel);

            var encodedJwt = _tokenService.CreateToken(claims);

            var response = new
            {
                access_token = encodedJwt
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationRequestModel userRequestModel)
        {
            var userModel = _mapper.Map<UserRegistrationModel>(userRequestModel);
            var registeredUserAuth = await _accountService.RegisterAsync(userModel);

            if (registeredUserAuth == null)
                return BadRequest(new { errorText = "User with such email exists" });

            return await Authenticate(_mapper.Map<UserAuthenticationRequestModel>(registeredUserAuth));
        }
    }
}
