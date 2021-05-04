using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
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
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate(UserResponseModel user)
        {
            var loggedUser = await _accountService.AuthenticateAsync(user.Email, user.Password);
            if (loggedUser == null)
                return BadRequest(new { errorText = "Invalid email or password" });

            var now = DateTime.UtcNow;

            var claims = await _accountService.GetIdentityAsync(user.Email, user.Password);
            var key =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AuthOptions:key"]));

            var jwt = new JwtSecurityToken(
                issuer: _configuration["AuthOptions:issuer"],
                audience: _configuration["AuthOptions:audience"],
                notBefore: now,
                claims: claims.Claims,
                expires: now.AddSeconds(double.Parse(_configuration["AuthOptions:lifetime"] + 10)),
                signingCredentials: new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationRequestModel user)
        {
            var registeredUser = await _accountService.RegisterAsync(user, user.Password);

            if (registeredUser == null)
                return BadRequest(new { errorText = "User with such email exists" });

            return await Authenticate(registeredUser);
        }
    }
}
