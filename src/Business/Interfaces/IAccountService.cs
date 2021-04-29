using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.RequestModels;

namespace HotelReservation.Business.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> AuthenticateAsync(string email, string password);
        Task<UserModel> RegisterAsync(UserRegistrationRequestModel user, string password);
        Task<ClaimsIdentity> GetIdentityAsync(string email, string password);
    }
}
