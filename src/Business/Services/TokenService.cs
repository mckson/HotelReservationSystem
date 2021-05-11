using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
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

        public string GenerateJwtToken(ClaimsIdentity claims)
        {
            var timeNow = DateTime.UtcNow;

            var securityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AuthOptions:key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _configuration["AuthOptions:issuer"],
                audience: _configuration["AuthOptions:audience"],
                claims: claims.Claims,
                expires: timeNow.AddMinutes(double.Parse(_configuration["AuthOptions:lifetime"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public RefreshTokenModel GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];

            rngCryptoServiceProvider.GetBytes(randomBytes);

            return new RefreshTokenModel
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddHours(5),
                Created = DateTime.UtcNow
            };
        }
    }
}
