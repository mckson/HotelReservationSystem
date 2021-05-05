using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IAccountService
    {
        Task<UserResponseModel> AuthenticateAsync(string email, string password);

        Task<UserResponseModel> RegisterAsync(UserRegistrationRequestModel user, string password);

        Task<ClaimsIdentity> GetIdentityAsync(string email, string password);
    }
}
