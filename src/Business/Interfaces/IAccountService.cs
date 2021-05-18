using System.Threading.Tasks;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;

namespace HotelReservation.Business.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> AuthenticateAsync(UserAuthenticationModel user);

        Task<UserAuthenticationModel> RegisterAsync(UserRegistrationModel user);

        Task<UserModel> RefreshToken(string token);

        Task RevokeTokenAsync(string token);
    }
}
