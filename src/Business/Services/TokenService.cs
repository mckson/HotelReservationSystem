using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelReservation.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TokenService(
            IConfiguration configuration,
            ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateJwtToken(ClaimsIdentity claims)
        {
            _logger.Debug("JWT token is generating");

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

            _logger.Debug("JWT token generated");

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public RefreshTokenModel GenerateRefreshToken()
        {
            _logger.Debug("Refresh token is generating");

            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];

            rngCryptoServiceProvider.GetBytes(randomBytes);

            _logger.Debug("Refresh token is generated");

            return new RefreshTokenModel
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddHours(5),
                Created = DateTime.UtcNow
            };
        }
    }
}
