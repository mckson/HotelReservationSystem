using System.Security.Claims;
using HotelReservation.Business.Models;

namespace HotelReservation.Business.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(ClaimsIdentity claims);

        RefreshTokenModel GenerateRefreshToken();
    }
}
