using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using Microsoft.Extensions.Options;
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
        private readonly ILogger _logger;
        private readonly AuthenticationOptions _authOptions;

        public TokenService(
            IOptions<AuthenticationOptions> authOptions,
            ILogger logger)
        {
            _authOptions = authOptions.Value;
            _logger = logger;
        }

        public string GenerateJwtToken(ClaimsIdentity claims)
        {
            _logger.Debug("JWT token is generating");

            var timeNow = DateTime.UtcNow;

            var securityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authOptions.Key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims.Claims,
                expires: timeNow.AddMinutes(_authOptions.Lifetime),
                signingCredentials: credentials);

            _logger.Debug("JWT token generated");

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public RefreshTokenEntity GenerateRefreshToken()
        {
            _logger.Debug("Refresh token is generating");

            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];

            rngCryptoServiceProvider.GetBytes(randomBytes);

            _logger.Debug("Refresh token is generated");

            return new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddHours(5),
                Created = DateTime.UtcNow
            };
        }
    }
}
