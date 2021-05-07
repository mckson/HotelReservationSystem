using System.Security.Claims;

namespace HotelReservation.Business.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ClaimsIdentity claims);
    }
}
