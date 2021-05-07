using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelReservation.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelReservation.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(ClaimsIdentity claims)
        {
            var timeNow = DateTime.UtcNow;

            var securityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AuthOptions:key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _configuration["AuthOptions:issuer"],
                audience: _configuration["AuthOptions:audience"],
                claims: claims.Claims,
                expires: timeNow.AddSeconds(double.Parse(_configuration["AuthOptions:lifetime"] + 10)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
