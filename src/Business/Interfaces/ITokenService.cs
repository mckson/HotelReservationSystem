using HotelReservation.Data.Entities;
using System.Security.Claims;

namespace HotelReservation.Business.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(ClaimsIdentity claims);

        RefreshTokenEntity GenerateRefreshToken();
    }
}
