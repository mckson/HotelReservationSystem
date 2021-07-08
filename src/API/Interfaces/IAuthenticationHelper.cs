using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.API.Interfaces
{
    public interface IAuthenticationHelper
    {
        Task<ClaimsIdentity> GetIdentityAsync(string email);
    }
}
