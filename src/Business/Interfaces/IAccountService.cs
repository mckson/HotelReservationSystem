using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.Business.Models.UserModels;

namespace HotelReservation.Business.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> AuthenticateAsync(UserAuthenticationModel user);

        Task<UserAuthenticationModel> RegisterAsync(UserRegistrationModel user);

        Task<ClaimsIdentity> GetIdentityAsync(UserAuthenticationModel user);
    }
}
