using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Data.Entities;
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
        public async Task<IActionResult> Authenticate(UserModel user)
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
                expires: now.Add(TimeSpan.FromMinutes(double.Parse(_configuration["AuthOptions:lifetime"]))),
                signingCredentials: new SigningCredentials(key,
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
            var registredUser = await _accountService.RegisterAsync(user, user.Password);

            if (registredUser == null)
                return BadRequest(new {errorText = "User with such email exists"});

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<UserRegistrationRequestModel, UserModel>());
            //var mapper = new Mapper(config);
            //var userModel = mapper.Map<UserRegistrationRequestModel, UserModel>(registredUser);

            return await Authenticate(registredUser);
        } 
    }
}
